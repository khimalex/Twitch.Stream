using System;

namespace Twitch.Libs.Exceptions.Configuration
{
    internal class RequiredConfigurationSectionNotEstablishedException : Exception
    {
        private static readonly String _errorMessageFormat = @"

Configuration file appsettings.jsom MUST contain section ""{0}""! For example: 
{
   ""{0}"": {
      ""ClientIDWeb"": ""kimne78kx3ncx6brgo4mv6wki5h1ko"",
      ""ClientID"": ""Your developer-token, received here: https://dev.twitch.tv/console/apps""
   }
}

";
        internal RequiredConfigurationSectionNotEstablishedException(String sectionName)
           : base(String.Format(_errorMessageFormat, sectionName))
        {
        }
    }
}
