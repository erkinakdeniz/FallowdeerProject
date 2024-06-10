using Core.Persistence.Repositories;

namespace Core.Security.Entities;

public class OtpAuthenticator : Entity<int>
{
    public Guid UserId { get; set; }
    public byte[] SecretKey { get; set; }
    public bool IsVerified { get; set; }
    public string ImageBase64 { get; set; }
    
    public virtual User User { get; set; } = null!;

    public OtpAuthenticator()
    {
        SecretKey = Array.Empty<byte>();
    }

   

    public OtpAuthenticator(int id, Guid userId, byte[] secretKey, bool isVerified)
        : base(id)
    {
        UserId = userId;
        SecretKey = secretKey;
        IsVerified = isVerified;
    }

    public OtpAuthenticator(Guid userId, byte[] secretKey, bool ısVerified, string ımageBase64)
    {
        UserId = userId;
        SecretKey = secretKey;
        IsVerified = ısVerified;
        ImageBase64 = ımageBase64;
    }
}
