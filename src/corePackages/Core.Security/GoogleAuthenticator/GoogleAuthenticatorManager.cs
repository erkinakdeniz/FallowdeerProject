using Core.Security.JWT;
using Google.Authenticator;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Security.GoogleAuthenticator;
public class GoogleAuthenticatorManager : IGoogleAuthenticatorService
{
    public GoogleAuthenticatorManager(IConfiguration configuration)
    {
        Configuration = configuration;
        const string configurationSection = "Authenticators";
        _authenticatorOptions =
            Configuration.GetSection(configurationSection).Get<AuthenticatorOptions>()
            ?? throw new NullReferenceException($"\"{configurationSection}\" section cannot found in configuration.");
    }

    public IConfiguration Configuration { get; }
    private AuthenticatorOptions _authenticatorOptions { get; }
    public GoogleAuthenticatorDto AuthenticateCreate()
    {
        TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
        var setupInfo = tfa.GenerateSetupCode(_authenticatorOptions.AppName, _authenticatorOptions.Description, _authenticatorOptions.GoogleAuthenticator.Key,false);
        return new GoogleAuthenticatorDto()
        {
            Base64Image = setupInfo.QrCodeSetupImageUrl,
            ManualEntryKey = setupInfo.ManualEntryKey
        };
    }
    public GoogleAuthenticatorDto AuthenticateCreate(string secretKey)
    {
        TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
        var setupInfo = tfa.GenerateSetupCode(_authenticatorOptions.AppName, _authenticatorOptions.Description, secretKey, false);
        return new GoogleAuthenticatorDto()
        {
            Base64Image = setupInfo.QrCodeSetupImageUrl,
            ManualEntryKey = setupInfo.ManualEntryKey
        };
    }

    public bool Verify(string pin)
    {
        TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
        bool isCorrectPIN = tfa.ValidateTwoFactorPIN(_authenticatorOptions.GoogleAuthenticator.Key, pin);
        return isCorrectPIN;
    }

    public bool Verify(string secretKey, string pin)
    {
        TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
        bool isCorrectPIN = tfa.ValidateTwoFactorPIN(secretKey, pin);
        return isCorrectPIN;
    }
}
