using System;

namespace Twitch.Libs.Dto;

public class VideoDto
{
    public string Id { get; set; }

    public string Game { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public string Type { get; set; }

    public string Language { get; set; }

    public string UserId { get; set; }

    public string UserLogin { get; set; }

    // public ChannelDto Channel { get; set; }

    public DateTime CreatedAt { get; set; }

}
