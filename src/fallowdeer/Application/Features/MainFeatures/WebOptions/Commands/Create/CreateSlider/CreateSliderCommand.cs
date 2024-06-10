using Application.Services.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Security.Entities;
using MediatR;
using static Application.Features.Constants.GeneralOperationClaims;

namespace Application.Features.MainFeatures.WebOptions.Commands.Create.CreateSlider;
public class CreateSliderCommand : IRequest<bool>, IRoleRequest
{
    public int CategoryId { get; set; }
    public List<CreateSliderRequest> Slides { get; set; }
    public CreateSliderCommand()
    {
        Slides = new List<CreateSliderRequest>();
    }

    public string[] Roles => new[] { Admin, SuperAdmin, Editor };

    public class CreateSliderCommandHandler : IRequestHandler<CreateSliderCommand,bool>
    {
        private readonly ISliderRepository _sliderRepository;
        private readonly IMapper _mapper;

        public CreateSliderCommandHandler(ISliderRepository sliderRepository, IMapper mapper)
        {
            _sliderRepository = sliderRepository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(CreateSliderCommand request, CancellationToken cancellationToken)
        {
            int order = 0;
            var lastSlider = _sliderRepository.Query(x => x.CategoryId == request.CategoryId, orderBy: x => x.OrderByDescending(x => x.Order)).FirstOrDefault();
            if (lastSlider != null)
                order = lastSlider.Order;

            foreach (var item in request.Slides)
            {
                Slider slider = _mapper.Map<Slider>(item);
                slider.CategoryId = request.CategoryId;
                slider.Order = ++order;
                await _sliderRepository.AddAsync(slider);
            }
            return true;

        }
    }
}
