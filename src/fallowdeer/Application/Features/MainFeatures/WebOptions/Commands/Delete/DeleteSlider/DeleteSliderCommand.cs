using Application.Features.MainFeatures.WebOptions.Rules;
using Application.Services.Repositories;
using Core.Application.Pipelines.Authorization;
using Core.Security.Entities;
using MediatR;
using static Application.Features.Constants.GeneralOperationClaims;

namespace Application.Features.MainFeatures.WebOptions.Commands.Delete.DeleteSlider;
public class DeleteSliderCommand : IRequest, IRoleRequest
{
    public int Id { get; set; }
    public string[] Roles => new[] { Admin, SuperAdmin, Editor };
    public class DeleteSliderCommandHandler : IRequestHandler<DeleteSliderCommand>
    {
        private readonly ISliderRepository _sliderRepository;
        private readonly SliderBusinessRules _sliderBusinessRules;

        public DeleteSliderCommandHandler(ISliderRepository sliderRepository, SliderBusinessRules sliderBusinessRules)
        {
            _sliderRepository = sliderRepository;
            _sliderBusinessRules = sliderBusinessRules;
        }

        public async Task Handle(DeleteSliderCommand request, CancellationToken cancellationToken)
        {
            Slider? slider = _sliderRepository.Get(x => x.Id == request.Id);
            await _sliderBusinessRules.WebOptionShouldBeExistsWhenSelected(slider);
            var deleted = _sliderRepository.Delete(slider!,true);
            if (deleted is not null)
            {
                var currentSliders = _sliderRepository.Query(x => x.CategoryId == deleted.CategoryId).ToList();
                int order = 0;
                foreach (var item in currentSliders)
                {
                    item.Order = ++order;
                    await _sliderRepository.UpdateAsync(item);
                }
            }
        }
    }
}
