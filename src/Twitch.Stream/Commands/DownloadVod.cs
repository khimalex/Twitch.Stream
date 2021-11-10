using Twitch.Libs.Dto;

namespace Twitch.Stream.Commands;

internal class DownloadVod : IApp
{
    private readonly Appsettings _options;
    private readonly ILogger _logger;
    private readonly IApiTwitchTv _apiTwitch;
    private readonly IUsherTwitchTv _usherTwitch;

    public DownloadVod(ILogger<DownloadVod> logger, IOptions<Appsettings> optionsAccessor, IApiTwitchTv apiTwitch, IUsherTwitchTv usherTwitch)
    {
        _options = optionsAccessor.Value;
        _logger = logger;
        _apiTwitch = apiTwitch;
        _usherTwitch = usherTwitch;
    }
    public async Task RunAsync(CancellationToken token = default)
    {
        IEnumerable<Task> tasks = _options.Streams.Select(async videoId =>
        {

            VideosDto videos = await _apiTwitch.GetVideoInfoAsync(videoId);
            if (!videos.VideoList.Any())
            {
                throw new Exception($"Не найдено видео '{videoId}'.");

            }

            VideoDto video = videos.VideoList.First();

            TwitchAuthDto vodTwitchAuth = await _apiTwitch.GetVodTwitchAuthAsync(videoId);

            string invalidFileName = $@"{video.Type} {video.UserLogin} {video.Game} {video.CreatedAt.ToLocalTime()}.m3u8";
            string validFileName = string.Join(" ", invalidFileName.Split(Path.GetInvalidFileNameChars()));

            var query = new GetVodQueryParams { Sig = vodTwitchAuth.Sig, Token = vodTwitchAuth.Token };

            HttpContent httpContent = await _usherTwitch.GetVodAsync(video.Id, query);

            byte[] bytes = await httpContent.ReadAsByteArrayAsync(token);

            await File.WriteAllBytesAsync(validFileName, bytes, token);

            const string message = "Видео '{ValidFileName}' загружено";
            _logger.LogInformation(message, validFileName);

        });

        await Task.WhenAll(tasks);
    }

}