using Application.Features.MainFeatures.WebOptions.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Core.Security.Entities;
using MediatR;

namespace Application.Features.MainFeatures.WebOptions.Queries.Sliders;
public class GetSliderQuery : IRequest<SliderQueryResponse>
{
    public int Id { get; set; }
    public class GetSliderQueryHandler : IRequestHandler<GetSliderQuery, SliderQueryResponse>
    {
        private readonly ISliderRepository _sliderRepository;
        private readonly IMapper _mapper;
        private readonly SliderBusinessRules _sliderBusinessRules;

        public GetSliderQueryHandler(ISliderRepository sliderRepository, IMapper mapper, SliderBusinessRules sliderBusinessRules)
        {
            _sliderRepository = sliderRepository;
            _mapper = mapper;
            _sliderBusinessRules = sliderBusinessRules;
        }

        public async Task<SliderQueryResponse> Handle(GetSliderQuery request, CancellationToken cancellationToken)
        {
            Slider? slider = _sliderRepository.Get(x => x.Id == request.Id,enableTracking:false);
            await _sliderBusinessRules.WebOptionShouldBeExistsWhenSelected(slider);
            SliderQueryResponse sliderQueryResponse = _mapper.Map<SliderQueryResponse>(slider);
            return sliderQueryResponse;
        }
    }
}
