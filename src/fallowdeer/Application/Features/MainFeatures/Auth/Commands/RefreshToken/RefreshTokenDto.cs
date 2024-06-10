namespace Application.Features.MainFeatures.Auth.Commands.RefreshToken;
public class RefreshTokenDto
{
    public string Token { get; set; }
    public string Referer { get; set; }
    public DateTime Expiration { get; set; }
}
