using Application.Services.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Core.Security.Entities;
using MediatR;
using static Application.Features.Constants.GeneralOperationClaims;

namespace Application.Features.MainFeatures.WebOptions.Commands.Update.UpdateSlider;
public class UpdateSliderCommand : IRequest<bool>,IRoleRequest
{
    public int CategoryId { get; set; }
    public List<UpdateSliderRequest> Slides { get; set; }

    public string[] Roles => new[] { Admin, SuperAdmin, Editor };

    public class UpdateSliderCommandHandler : IRequestHandler<UpdateSliderCommand,bool>
    {
        private readonly ISliderRepository _sliderRepository;
        private readonly IMapper _mapper;

        public UpdateSliderCommandHandler(ISliderRepository sliderRepository, IMapper mapper)
        {
            _sliderRepository = sliderRepository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateSliderCommand request, CancellationToken cancellationToken)
        {

            var errors = new List<OperationFailedExceptionModel>();
            foreach (var item in request.Slides)
            {
                
                Slider? slider = _sliderRepository.Get(x => x.Id == item.Id);
                if (slider is null)
                {
                    var operationFailedExceptionModel = new OperationFailedExceptionModel();
                    operationFailedExceptionModel.Id = item.Id.ToString();
                    operationFailedExceptionModel.Name = item.Image;
                    operationFailedExceptionModel.Description = "Slayt Güncellenemedi";
                    errors.Add(operationFailedExceptionModel);
                    continue;
                }
                var mappedslider = _mapper.Map(item, slider);
                mappedslider.CategoryId = request.CategoryId;
                await _sliderRepository.UpdateAsync(mappedslider);
            }
            if (errors.Count > 0)
                throw new OperationFailedException("Bazı Slaytlar Güncellenemedi!", errors);

            return true;


        }
    }
}
