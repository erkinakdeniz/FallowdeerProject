using Application.Features.MainFeatures.WebOptions.Commands.Create.CreateSlider;
using Application.Features.MainFeatures.WebOptions.Commands.Update.UpdateSlider;
using Application.Features.MainFeatures.WebOptions.Queries.SliderCategory;
using Application.Features.MainFeatures.WebOptions.Queries.Sliders;
using Application.Features.MainFeatures.WebOptions.Queries.WebOptions;
using AutoMapper;
using Core.Security.Entities;

namespace Application.Features.MainFeatures.WebOptions.Profiles;
public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<GetWebOptionsResponse, WebOption>()
            .ForPath(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForPath(dest => dest.Key, opt => opt.MapFrom(src => src.Title))
            .ForPath(dest => dest.Value, opt => opt.MapFrom(src => src.Content))
            .ForPath(dest => dest.Alias, opt => opt.MapFrom(src => src.Alias))
            .ForPath(dest => dest.InputType, opt => opt.MapFrom(src => src.InputType))
            .ForPath(dest => dest.Params, opt => opt.MapFrom(src => src.Params))
            .ReverseMap();
        CreateMap<CreateSliderCommand, Slider>().ReverseMap();
        CreateMap<UpdateSliderCommand, Slider>().ReverseMap();
        CreateMap<SliderQueryResponse, Slider>().ReverseMap();
        CreateMap<CreateSliderRequest, Slider>().ReverseMap();
        CreateMap<UpdateSliderRequest, Slider>().ReverseMap();
        CreateMap<SCategoryResponse, SCategory>().ReverseMap();
    }
}
