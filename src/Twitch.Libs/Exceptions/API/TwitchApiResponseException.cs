using System;
using Twitch.Libs.API.CommonModels;

namespace Twitch.Libs.Exceptions.API;

internal class TwitchApiResponseException : Exception
{

    internal TwitchApiResponseException(string message, ErrorResponse errorResponse) : base($"{Environment.NewLine}{Environment.NewLine}{message.TrimEnd('.')}.{Environment.NewLine}Due to Twitch Api Error: Status: {errorResponse.Status}, Error: {errorResponse.Error}, Message: {errorResponse.Message}.{Environment.NewLine}")
    {

    }
}
