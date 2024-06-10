using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

namespace Core.Security.Encryption;

public static class SecurityKeyHelper
{
    public static SecurityKey CreateSecurityKey(string securityKey) => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
    public static SecurityKey SHA512Bit(string input)
    {
        var bytes = Encoding.UTF8.GetBytes(input);
        using (var hash = SHA512.Create())
        {
            var hashedInputBytes = hash.ComputeHash(bytes);
            return new SymmetricSecurityKey(hashedInputBytes);
        }
    }
}
