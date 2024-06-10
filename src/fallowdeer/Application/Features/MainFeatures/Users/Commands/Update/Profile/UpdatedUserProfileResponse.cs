using Core.Application.Responses;
using Core.Security.JWT;

namespace Application.Features.MainFeatures.Users.Commands.Update.Profile;

public class UpdatedUserProfileResponse : IResponse
{
    public string Token { get; set; }
    public string Referer { get; set; }
    public DateTime Expiration { get; set; }
}
