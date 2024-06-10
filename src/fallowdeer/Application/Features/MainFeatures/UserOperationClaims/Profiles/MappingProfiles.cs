using Application.Features.MainFeatures.UserOperationClaims.Commands.Create;
using Application.Features.MainFeatures.UserOperationClaims.Commands.Delete;
using Application.Features.MainFeatures.UserOperationClaims.Commands.Update;
using Application.Features.MainFeatures.UserOperationClaims.Queries.GetById;
using Application.Features.MainFeatures.UserOperationClaims.Queries.GetByUserId;
using Application.Features.MainFeatures.UserOperationClaims.Queries.GetList;
using AutoMapper;
using Core.Application.Responses;
using Core.Persistence.Paging;
using Core.Security.Entities;

namespace Application.Features.MainFeatures.UserOperationClaims.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<UserOperationClaim, CreateUserOperationClaimCommand>().ReverseMap();
        CreateMap<UserOperationClaim, CreatedUserOperationClaimResponse>().ReverseMap();
        CreateMap<UserOperationClaim, UpdateUserOperationClaimCommand>().ReverseMap();
        CreateMap<UserOperationClaim, UpdatedUserOperationClaimResponse>().ReverseMap();
        CreateMap<UserOperationClaim, DeleteUserOperationClaimCommand>().ReverseMap();
        CreateMap<UserOperationClaim, DeletedUserOperationClaimResponse>().ReverseMap();
        CreateMap<UserOperationClaim, GetByIdUserOperationClaimResponse>().ReverseMap();
        CreateMap<UserOperationClaim, GetByUserIdUserOperationClaimResponse>()
            .ForPath(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForPath(dest => dest.Name, opt => opt.MapFrom(src => src.OperationClaim.Name))
            .ForPath(dest => dest.OperationClaimId, opt => opt.MapFrom(src => src.OperationClaimId))
            .ReverseMap();
        CreateMap<UserOperationClaim, GetListUserOperationClaimListItemDto>().ReverseMap();
        CreateMap<IPaginate<UserOperationClaim>, GetListResponse<GetListUserOperationClaimListItemDto>>().ReverseMap();
    }
}
