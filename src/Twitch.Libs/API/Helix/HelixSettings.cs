namespace Twitch.Libs.API.Helix
{
    public class HelixSettings
    {
        public readonly string TwitchGQL = @"https://gql.twitch.tv/gql";
        public readonly string AuthorizationAndAuthentification = @"https://id.twitch.tv";
        public readonly string Api = @"https://api.twitch.tv/helix";
        public string ClientID { get; set; }
        public string ClientIDWeb { get; set; }
        public string ClientSecret { get; set; }
    }
}
