using System;

namespace Twitch.Libs.Exceptions.Configuration
{
    internal class RequiredConfigurationSectionNotEstablishedException : Exception
    {
        private static readonly String _errorMessageFormat = @"Configuration file appsettings.json MUST contain section ""{0}""!";
        internal RequiredConfigurationSectionNotEstablishedException(String sectionName)
           : base(String.Format(_errorMessageFormat, sectionName))
        {
        }
    }
}
