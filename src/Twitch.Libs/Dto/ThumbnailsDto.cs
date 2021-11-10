using System.Collections.Generic;

namespace Twitch.Libs.Dto;

public class ThumbnailsDto
{
    public List<ThumbnailDto> Large { get; set; }

    public List<ThumbnailDto> Medium { get; set; }

    public List<ThumbnailDto> Small { get; set; }

    public List<ThumbnailDto> Template { get; set; }

}
