using Application.Services.Repositories;
using AutoMapper;
using MediatR;

namespace Application.Features.MainFeatures.WebOptions.Queries.Sliders;
public class GetByCategorySlidesQuery : IRequest<List<SliderQueryResponse>>
{
    public int CategoryId { get; set; }
    public class GetByCategorySlidesQueryHandler : IRequestHandler<GetByCategorySlidesQuery, List<SliderQueryResponse>>
    {
        private readonly ISliderRepository _sliderRepository;
        private readonly IMapper _mapper;

        public GetByCategorySlidesQueryHandler(ISliderRepository sliderRepository, IMapper mapper)
        {
            _sliderRepository = sliderRepository;
            _mapper = mapper;
        }

        public async Task<List<SliderQueryResponse>> Handle(GetByCategorySlidesQuery request, CancellationToken cancellationToken)
        {
            var slides = await _sliderRepository.QueryAsync(x => x.CategoryId == request.CategoryId, orderBy: x => x.OrderBy(x => x.Order));
            List<SliderQueryResponse> sliderQueryResponses = new();
            if (slides is not null)
                sliderQueryResponses = _mapper.Map<List<SliderQueryResponse>>(slides);

            return sliderQueryResponses;
        }
    }
}
