using System;

namespace Twitch.Libs.API.Kraken
{
    public class KrakenSettings
    {
        public readonly String TwitchGQL = @"https://gql.twitch.tv/gql";
        public readonly String Api = @"https://api.twitch.tv/kraken";
        public String ClientID { get; set; }
        public String ClientIDWeb { get; set; }
    }
}
