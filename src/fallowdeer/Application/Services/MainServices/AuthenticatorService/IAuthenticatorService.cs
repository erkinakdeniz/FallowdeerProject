using Core.Security.Entities;
using Core.Security.Enums;

namespace Application.Services.MainServices.AuthenticatorService;

public interface IAuthenticatorService
{
    public Task<EmailAuthenticator> CreateEmailAuthenticator(User user);
    public Task<OtpAuthenticator> CreateOtpAuthenticator(User user);
    public Task<string> ConvertSecretKeyToString(byte[] secretKey);
    public Task SendAuthenticatorCode(User user, AuthenticatorType authenticatorType);
    public Task VerifyAuthenticatorCode(User user, string authenticatorCode, AuthenticatorType authenticatorType);
}
