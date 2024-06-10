namespace Core.Security.OtpAuthenticator;

public interface IOtpAuthenticatorHelper
{
    public Task<byte[]> GenerateSecretKey();
    public byte[] GenerateDateKey(int months);
    public Task<string> ConvertSecretKeyToString(byte[] secretKey);
    public DateTime ConvertToDate(byte[] base32Bytes);
    public Task<bool> VerifyCode(byte[] secretKey, string code);
}
