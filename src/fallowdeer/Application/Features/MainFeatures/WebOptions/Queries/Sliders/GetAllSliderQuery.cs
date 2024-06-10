using Application.Services.Repositories;
using AutoMapper;
using MediatR;

namespace Application.Features.MainFeatures.WebOptions.Queries.Sliders;
public class GetAllSliderQuery : IRequest<List<SliderQueryResponse>>
{
    public class GetAllSliderQueryHandler : IRequestHandler<GetAllSliderQuery, List<SliderQueryResponse>>
    {
        private readonly ISliderRepository _sliderRepository;
        private readonly IMapper _mapper;

        public GetAllSliderQueryHandler(ISliderRepository sliderRepository, IMapper mapper)
        {
            _sliderRepository = sliderRepository;
            _mapper = mapper;
        }

        public async Task<List<SliderQueryResponse>> Handle(GetAllSliderQuery request, CancellationToken cancellationToken)
        {
            var slides = await _sliderRepository.QueryAsync(x => x.Visible == true, orderBy: x => x.OrderBy(x => x.Order));
            List<SliderQueryResponse> sliderQueryResponses = new();
            if (slides is not null)
                sliderQueryResponses = _mapper.Map<List<SliderQueryResponse>>(slides);
            return sliderQueryResponses;

        }
    }
}
