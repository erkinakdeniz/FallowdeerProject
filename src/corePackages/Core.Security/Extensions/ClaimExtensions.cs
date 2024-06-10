using Core.CrossCuttingConcerns.Extensions;
using System.Security.Claims;

namespace Core.Security.Extensions;

public static class ClaimExtensions
{
    public static void AddEmail(this ICollection<Claim> claims, string email) =>
        claims.Add(new Claim(MyClaimTypes.Email, email));
    //JwtRegisteredClaimNames.Email
    public static void AddName(this ICollection<Claim> claims, string name) => claims.Add(new Claim(MyClaimTypes.Name, name));
    //ClaimTypes.Name
    public static void AddNameIdentifier(this ICollection<Claim> claims, string nameIdentifier) =>
        claims.Add(new Claim(MyClaimTypes.ID, nameIdentifier));
    //ClaimTypes.NameIdentifier
    public static void AddRoles(this ICollection<Claim> claims, string[] roles) =>
        roles.ToList().ForEach(role => claims.Add(new Claim(MyClaimTypes.Role, role)));
    //ClaimTypes.Role
}
