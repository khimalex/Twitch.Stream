using Twitch.Libs.Dto;

namespace Twitch.Stream.Commands;

internal class DownloadStreams : IApp
{
    private readonly Appsettings _optionsAccessor;
    private readonly ILogger<DownloadStreams> _logger;
    private readonly IApiTwitchTv _apiTwitch;
    private readonly IUsherTwitchTv _usherTwitch;

    public DownloadStreams(ILogger<DownloadStreams> logger, IOptions<Appsettings> optionsAccessor, IApiTwitchTv apiTwitch, IUsherTwitchTv usherTwitch)
    {
        _optionsAccessor = optionsAccessor.Value;
        _logger = logger;
        _apiTwitch = apiTwitch;
        _usherTwitch = usherTwitch;
    }
    public async Task RunAsync(CancellationToken token = default)
    {
        IEnumerable<Task> tasks = _optionsAccessor.Streams.Select(async user =>
        {
            try
            {
                string fileName = $"{user}.m3u8";
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                TwitchAuthDto twitchauth = await _apiTwitch.GetChannelTwitchAuthAsync(user);

                var query = new GetStreamQueryParams
                {
                    Sig = twitchauth.Sig,
                    Token = twitchauth.Token
                };

                HttpContent httpContent = await _usherTwitch.GetStreamAsync(user, query);

                byte[] bytes = await httpContent.ReadAsByteArrayAsync(token);

                await File.WriteAllBytesAsync(fileName, bytes);

                const string message = "Стрим '{User}' загружен в '{File}'!";
                _logger.LogInformation(message, user, fileName);
            }
            catch (Exception e)
            {
                const string message = "Ошибка загрузки стрима '{User}': {Message}";
                _logger.LogError(message, user, e.Message);
            }

        });

        await Task.WhenAll(tasks);

    }

}