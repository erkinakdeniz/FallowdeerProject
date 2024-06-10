using Application.Features.MainFeatures.WebOptions.Queries.Sliders;
using Application.Services.Repositories;
using AutoMapper;
using MediatR;

namespace Application.Features.MainFeatures.Website.Queries.Sliders;
public class SlidesShowQuery : IRequest<List<SliderQueryResponse>>
{
    public int CategoryId { get; set; }
    public class SlidesShowQueryHandler : IRequestHandler<SlidesShowQuery, List<SliderQueryResponse>>
    {
        private readonly ISliderRepository _sliderRepository;
        private readonly IMapper _mapper;

        public SlidesShowQueryHandler(ISliderRepository sliderRepository, IMapper mapper)
        {
            _sliderRepository = sliderRepository;
            _mapper = mapper;
        }

        public async Task<List<SliderQueryResponse>> Handle(SlidesShowQuery request, CancellationToken cancellationToken)
        {
            var slides = await _sliderRepository.QueryAsync(x => x.CategoryId == request.CategoryId && x.Visible == true, orderBy: x => x.OrderBy(x => x.Order),enableTracking:false);
            List<SliderQueryResponse> sliderQueryResponses = new();
            if (slides is not null)
                sliderQueryResponses = _mapper.Map<List<SliderQueryResponse>>(slides);

            return sliderQueryResponses;
        }
    }
}
