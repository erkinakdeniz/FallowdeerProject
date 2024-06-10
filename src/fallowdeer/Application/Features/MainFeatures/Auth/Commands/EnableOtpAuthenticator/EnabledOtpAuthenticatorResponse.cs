using Core.Application.Responses;

namespace Application.Features.MainFeatures.Auth.Commands.EnableOtpAuthenticator;

public class EnabledOtpAuthenticatorResponse : IResponse
{
    public string SecretKey { get; set; }
    public string ImageBase64 { get; set; }

    public EnabledOtpAuthenticatorResponse()
    {
        SecretKey = string.Empty;
    }

    public EnabledOtpAuthenticatorResponse(string secretKey, string ımageBase64)
    {
        SecretKey = secretKey;
        ImageBase64 = ımageBase64;
    }
}
