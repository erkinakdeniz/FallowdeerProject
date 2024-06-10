using Core.Application.Responses;
using Core.Security.JWT;
using static Application.Features.MainFeatures.Auth.Commands.Login.LoggedResponse;

namespace Application.Features.MainFeatures.Auth.Commands.Register;

public class RegisteredResponse : IResponse
{
    public AccessToken AccessToken { get; set; }
    public Core.Security.Entities.RefreshToken RefreshToken { get; set; }

    public RegisteredResponse()
    {
        AccessToken = null!;
        RefreshToken = null!;
    }
    public LoggedHttpResponse ToHttpResponse()
    {
        if (AccessToken != null)
            return new() { Token = AccessToken.Token, Referer = RefreshToken?.Token, Expiration = RefreshToken.Expires, RequiredAuthenticatorType = Core.Security.Enums.AuthenticatorType.None };

        return new() { RequiredAuthenticatorType = Core.Security.Enums.AuthenticatorType.None };
    }

    public RegisteredResponse(AccessToken accessToken, Core.Security.Entities.RefreshToken refreshToken)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
}
