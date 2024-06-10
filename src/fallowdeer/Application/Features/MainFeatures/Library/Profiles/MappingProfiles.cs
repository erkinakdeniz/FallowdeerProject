using Application.Features.MainFeatures.Library.Queries.Files;
using Application.Services.MainServices.FileService;
using AutoMapper;

namespace Application.Features.MainFeatures.Library.Profiles;
public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<LibraryFileForPopupResponse, FileInfoDto>().ReverseMap();
    }
}
