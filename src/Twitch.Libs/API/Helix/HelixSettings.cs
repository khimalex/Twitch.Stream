using System;

namespace Twitch.Libs.API.Helix
{
    public class HelixSettings
    {
        public readonly String TwitchGQL = @"https://gql.twitch.tv/gql";
        public readonly String AuthorizationAndAuthentification = @"https://id.twitch.tv";
        public readonly String Api = @"https://api.twitch.tv/helix";
        public String ClientID { get; set; }
        public String ClientIDWeb { get; set; }
        public String ClientSecret { get; set; }
    }
}
