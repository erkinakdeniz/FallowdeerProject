using Application.Features.MainFeatures.Users.Commands.Create;
using Application.Features.MainFeatures.Users.Commands.Delete;
using Application.Features.MainFeatures.Users.Commands.Update;
using Application.Features.MainFeatures.Users.Commands.Update.Profile;
using Application.Features.MainFeatures.Users.Dtos;
using Application.Features.MainFeatures.Users.Queries.GetById;
using Application.Features.MainFeatures.Users.Queries.GetList;
using AutoMapper;
using Core.Application.Responses;
using Core.Persistence.Paging;
using Core.Security.Entities;

namespace Application.Features.MainFeatures.Users.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<User, CreateUserCommand>().ReverseMap();
        CreateMap<User, CreatedUserResponse>().ReverseMap();
        CreateMap<User, UpdateUserCommand>().ReverseMap();
        CreateMap<User, UpdatedUserResponse>().ReverseMap();
        CreateMap<User, UpdateUserProfileCommand>().ReverseMap();
        CreateMap<User, UpdatedUserProfileResponse>().ReverseMap();
        CreateMap<User, DeleteUserCommand>().ReverseMap();
        CreateMap<User, DeletedUserResponse>().ReverseMap();
        CreateMap<User, GetByIdUserResponse>().ReverseMap();
        CreateMap<RoleDto, UserOperationClaim>()
            .ForPath(dest => dest.OperationClaimId, opt => opt.MapFrom(src => src.Id))
            .ForPath(dest => dest.OperationClaim.Name, opt => opt.MapFrom(src => src.Name))
            .ReverseMap();
        CreateMap<User, GetListUserListItemDto>().ForPath(dest => dest.Roles, opt => opt.MapFrom(src => src.UserOperationClaims)).ReverseMap();
        CreateMap<IPaginate<User>, GetListResponse<GetListUserListItemDto>>().ReverseMap();
    }
}
