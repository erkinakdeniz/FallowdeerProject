using Application.Features.MainFeatures.Auth.Commands.RevokeToken;
using AutoMapper;
using Core.Security.Entities;

namespace Application.Features.MainFeatures.Auth.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<RefreshToken, RevokedTokenResponse>().ReverseMap();
    }
}
