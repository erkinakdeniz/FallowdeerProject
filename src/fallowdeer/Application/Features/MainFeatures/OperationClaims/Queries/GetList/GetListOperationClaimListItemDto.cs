using Core.Application.Dtos;

namespace Application.Features.MainFeatures.OperationClaims.Queries.GetList;

public class GetListOperationClaimListItemDto : IDto
{
    public int Id { get; set; }
    public string Name { get; set; }

    public GetListOperationClaimListItemDto()
    {
        Name = string.Empty;
    }

    public GetListOperationClaimListItemDto(int id, string name)
    {
        Id = id;
        Name = name;
    }
}
