using Core.Application.Responses;
using Core.Security.Enums;
using Core.Security.JWT;

namespace Application.Features.MainFeatures.Auth.Commands.Login;

public class LoggedResponse : IResponse
{
    public AccessToken? AccessToken { get; set; }
    public Core.Security.Entities.RefreshToken? RefreshToken { get; set; }
    public AuthenticatorType? RequiredAuthenticatorType { get; set; }

    public LoggedHttpResponse ToHttpResponse()
    {
        if (AccessToken != null)
            return new() { Token = AccessToken.Token, Referer = RefreshToken?.Token, Expiration = RefreshToken.Expires, RequiredAuthenticatorType = RequiredAuthenticatorType };

        return new() { RequiredAuthenticatorType = RequiredAuthenticatorType };
    }


    public class LoggedHttpResponse
    {
        //public AccessToken? AccessToken { get; set; }
        public string? Token { get; set; }
        public string? Referer { get; set; }
        public DateTime? Expiration { get; set; }
        public AuthenticatorType? RequiredAuthenticatorType { get; set; }
    }
}
