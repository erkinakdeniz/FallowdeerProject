using Core.Application.Responses;
using Core.Security.JWT;
using static Application.Features.MainFeatures.Auth.Commands.Login.LoggedResponse;

namespace Application.Features.MainFeatures.Auth.Commands.RefreshToken;

public class RefreshedTokensResponse : IResponse
{
    public AccessToken AccessToken { get; set; }
    public Core.Security.Entities.RefreshToken Referer { get; set; }

    public RefreshedTokensResponse()
    {
        AccessToken = null!;
        Referer = null!;
    }
    public RefreshTokenDto ToHttpResponse() =>
       new() { Token = AccessToken.Token, Referer = Referer.Token, Expiration = Referer.Expires };

    public RefreshedTokensResponse(AccessToken accessToken, Core.Security.Entities.RefreshToken refreshToken)
    {
        AccessToken = accessToken;
        Referer = refreshToken;
    }
}
