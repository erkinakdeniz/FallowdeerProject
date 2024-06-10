using OtpNet;

namespace Core.Security.OtpAuthenticator.OtpNet;

public class OtpNetOtpAuthenticatorHelper : IOtpAuthenticatorHelper
{
    public Task<byte[]> GenerateSecretKey()
    {
        byte[] key = KeyGeneration.GenerateRandomKey(20);

        string base32String = Base32Encoding.ToString(key);
        byte[] base32Bytes = Base32Encoding.ToBytes(base32String);

        return Task.FromResult(base32Bytes);
    }
    public byte[] GenerateDateKey(int months)
    {
        DateTime date = DateTime.Now.ToUniversalTime().AddMonths(months);
        byte[] key = BitConverter.GetBytes(date.ToBinary());

        string base32String = Base32Encoding.ToString(key);
        byte[] base32Bytes = Base32Encoding.ToBytes(base32String);
        
        return base32Bytes;
    }
    public DateTime ConvertToDate(byte[] base32Bytes)
    {
        // Base32 bytes dizisini stringe dönüştür
        string base32String = Base32Encoding.ToString(base32Bytes);

        // Base32 stringini byte[] dizisine dönüştür
        byte[] key = Base32Encoding.ToBytes(base32String);

        // Byte[] dizisini long sayısına dönüştür
        long binary = BitConverter.ToInt64(key);

        // Long sayısını DateTime değerine dönüştür
        DateTime date = DateTime.FromBinary(binary);

        // DateTime değerini geri döndür
        return date;
    }

    public Task<string> ConvertSecretKeyToString(byte[] secretKey)
    {
        string base32String = Base32Encoding.ToString(secretKey);
        return Task.FromResult(base32String);
    }

    public Task<bool> VerifyCode(byte[] secretKey, string code)
    {
        Totp totp = new(secretKey);
       

        string totpCode = totp.ComputeTotp(DateTime.UtcNow);

        bool result = totpCode == code;

        return Task.FromResult(result);
    }
}
