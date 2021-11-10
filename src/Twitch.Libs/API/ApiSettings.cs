using Twitch.Libs.API.Helix;
using Twitch.Libs.API.Usher;

namespace Twitch.Libs.API;

public class ApiSettings
{
    // public readonly String TwitchGQL = @"https://gql.twitch.tv/gql";
    // public String ClientID { get; set; }
    // public String ClientIDWeb { get; set; }
    // public String ClientSecret { get; set; }

    public HelixSettings HelixSettings { get; set; }
    public UsherSettings UsherSettings { get; set; }
}
