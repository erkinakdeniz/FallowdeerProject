using Core.Application.Responses;

namespace Application.Features.MainFeatures.UserOperationClaims.Commands.Delete;

public class DeletedUserOperationClaimResponse : IResponse
{
    public int Id { get; set; }
}
