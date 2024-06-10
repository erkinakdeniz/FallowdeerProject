using Core.Security.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace Core.Security.JWT;

public interface ITokenHelper
{
    AccessToken CreateToken(User user, IList<OperationClaim> operationClaims);

    RefreshToken CreateRefreshToken(User user, string ipAddress);
    JwtSecurityToken TokenVerify(string token);
}
