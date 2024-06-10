using System.Security.Cryptography;
using System.Text;

namespace Core.Security.Cryptographies;
public class EllipticCurveCryptography : IECCCryptography
{
    public EllipticCurveCryptographyEncryptedResponse Encryption(string value)
    {
        using (ECDiffieHellmanCng keyExchange = new ECDiffieHellmanCng())
        {
            byte[] publicKey = keyExchange.PublicKey.ToByteArray();
            byte[] sharedSecret = keyExchange.DeriveKeyMaterial(CngKey.Import(publicKey, CngKeyBlobFormat.EccPublicBlob));

            // Simulate encryption (you can replace this with your actual encryption logic)
            byte[] encryptedData = EncryptData(value, sharedSecret, out byte[] tag, out byte[] iv);

            return new EllipticCurveCryptographyEncryptedResponse
            {
                EncryptedValue = Convert.ToBase64String(encryptedData),
                IV = Convert.ToBase64String(iv),
                Tag = Convert.ToBase64String(tag),
                Key = Convert.ToBase64String(sharedSecret)
            };
        }
    }
    public EllipticCurveCryptographyDecryptedResponse Decryption(string encryptedValue, string iv, string tag, string key)
    {
      
        byte[] encryptedData = Convert.FromBase64String(encryptedValue);
        byte[] ivBytes = Convert.FromBase64String(iv);
        byte[] tagBytes = Convert.FromBase64String(tag);
        byte[] keyBytes = Convert.FromBase64String(key);

    
        string decryptedValue = DecryptData(encryptedData, keyBytes, tagBytes, ivBytes);

        return new EllipticCurveCryptographyDecryptedResponse
        {
            Value = decryptedValue
        };
    }
    private byte[] EncryptData(string data, byte[] key, out byte[] tag, out byte[] iv)
    {
        using (AesGcm aesGcm = new AesGcm(key))
        {
            // Generate a random IV (Initialization Vector)
            iv = new byte[12]; // 96-bit IV
            RandomNumberGenerator.Fill(iv);

            // Convert the data to a byte array
            byte[] plaintext = Encoding.UTF8.GetBytes(data);

            // Encryption process
            byte[] ciphertext = new byte[plaintext.Length];
            tag = new byte[16]; // 128-bit tag
            aesGcm.Encrypt(iv, plaintext, ciphertext, tag);

            return ciphertext;
        }
    }


    private string DecryptData(byte[] encryptedData, byte[] key, byte[] tag, byte[] iv)
    {
        // ECC algorithm for decrypting data
        using (AesGcm aesGcm = new AesGcm(key))
        {
            byte[] decryptedData = new byte[encryptedData.Length];
            aesGcm.Decrypt(iv, encryptedData, tag, decryptedData); // Decrypt the data

            return Encoding.UTF8.GetString(decryptedData); // Return the decrypted data as a string
        }
    }
}

