namespace Core.Security.Cryptographies;
public interface IECCCryptography
{
    EllipticCurveCryptographyEncryptedResponse Encryption(string value);
    EllipticCurveCryptographyDecryptedResponse Decryption(string encryptedValue, string iv, string tag, string key);
}
