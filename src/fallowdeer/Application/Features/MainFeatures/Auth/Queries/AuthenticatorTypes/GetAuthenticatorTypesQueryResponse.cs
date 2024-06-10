using Core.Application.Responses;

namespace Application.Features.MainFeatures.Auth.Queries.AuthenticatorTypes;
public class GetAuthenticatorTypesQueryResponse : IResponse
{
    public GetAuthenticatorTypesQueryResponse(EmailAuthenticatorResponse emailAuthenticator, OtpAuthenticatorResponse otpAuthenticator, SmsAuthenticatorResponse smsAuthenticator)
    {
        EmailAuthenticator = emailAuthenticator;
        OtpAuthenticator = otpAuthenticator;
        SmsAuthenticator = smsAuthenticator;
    }
    public GetAuthenticatorTypesQueryResponse()
    {

    }

    public EmailAuthenticatorResponse EmailAuthenticator { get; set; }
    public OtpAuthenticatorResponse OtpAuthenticator { get; set; }
    public SmsAuthenticatorResponse SmsAuthenticator { get; set; }

    public class EmailAuthenticatorResponse
    {
        public bool IsOpen { get; set; } = false;
        public bool IsVerify { get; set; } = false;
        public string Email { get; set; }

    }
    public class OtpAuthenticatorResponse
    {
        public bool IsOpen { get; set; } = false;
        public bool IsVerify { get; set; } = false;
        public string OtpSecretKey { get; set; }
        public string OtpImageBase64 { get; set; }

    }
    public class SmsAuthenticatorResponse
    {
        public bool IsOpen { get; set; } = false;
        public bool IsVerify { get; set; } = false;
        public string Phone { get; set; }


    }

}
