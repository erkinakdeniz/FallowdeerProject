using Core.Persistence.Repositories;

namespace Core.Security.Entities;

public class UserOperationClaim : Entity<int>
{
    public Guid UserId { get; set; }
    public int OperationClaimId { get; set; }

    public virtual User User { get; set; } = null!;
    public virtual OperationClaim OperationClaim { get; set; } = null!;

    public UserOperationClaim(Guid userId, int operationClaimId)
    {
        UserId = userId;
        OperationClaimId = operationClaimId;
    }
  

    public UserOperationClaim(int id, Guid userId, int operationClaimId)
        : base(id)
    {
        UserId = userId;
        OperationClaimId = operationClaimId;
    }
}
