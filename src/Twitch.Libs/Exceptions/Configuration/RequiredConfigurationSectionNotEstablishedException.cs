using System;

namespace Twitch.Libs.Exceptions.Configuration;

internal class RequiredConfigurationSectionNotEstablishedException : Exception
{
    private const string _errorMessageFormat = @"Configuration file appsettings.json MUST contain section ""{0}""!";
    internal RequiredConfigurationSectionNotEstablishedException(string sectionName)
       : base(string.Format(_errorMessageFormat, sectionName))
    {
    }
}
