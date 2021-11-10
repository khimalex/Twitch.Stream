using System.Collections.Generic;
using Newtonsoft.Json;

namespace Twitch.Libs.API.CommonModels;

public class Video
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("user_id")]
    public string UserId { get; set; }

    [JsonProperty("user_name")]
    public string UserName { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("created_at")]
    public string CreatedAt { get; set; }

    [JsonProperty("published_at")]
    public string PublishedAt { get; set; }

    [JsonProperty("url")]
    public string Url { get; set; }

    [JsonProperty("thumbnail_url")]
    public string ThumbnailUrl { get; set; }

    [JsonProperty("viewable")]
    public string Viewable { get; set; }

    [JsonProperty("view_count")]
    public long ViewCount { get; set; }

    [JsonProperty("language")]
    public string Language { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("duration")]
    public string Duration { get; set; }

    [JsonProperty("broadcast_id")]
    public long BroadcastId { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }

    [JsonProperty("tag_list")]
    public string TagList { get; set; }

    [JsonProperty("_id")]
    public string TwitchId { get; set; }

    [JsonProperty("recorded_at")]
    public string RecordedAt { get; set; }

    [JsonProperty("game")]
    public string Game { get; set; }

    [JsonProperty("length")]
    public int Length { get; set; }

    [JsonProperty("preview")]
    public string Preview { get; set; }

    [JsonProperty("views")]
    public int Views { get; set; }

    [JsonProperty("broadcast_type")]
    public string BroadcastType { get; set; }

    [JsonProperty("_links")]
    public Links Links { get; set; }

    [JsonProperty("channel")]
    public Channel Channel { get; set; }

    [JsonProperty("animated_preview")]
    public string AnimatedPreview { get; set; }

    [JsonProperty("thumbnails")]
    public List<Thumbnail> Thumbnails { get; set; }

    [JsonProperty("fps")]
    public Fps Fps { get; set; }

    [JsonProperty("resolutions")]
    public Resolutions Resolutions { get; set; }

}
