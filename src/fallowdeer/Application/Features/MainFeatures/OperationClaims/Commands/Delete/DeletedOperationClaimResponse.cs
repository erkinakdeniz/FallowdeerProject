using Core.Application.Responses;

namespace Application.Features.MainFeatures.OperationClaims.Commands.Delete;

public class DeletedOperationClaimResponse : IResponse
{
    public int Id { get; set; }
}
