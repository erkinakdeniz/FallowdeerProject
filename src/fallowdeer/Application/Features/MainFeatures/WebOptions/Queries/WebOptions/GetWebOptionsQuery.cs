using Application.Services.MainServices.WebOptionService;
using AutoMapper;
using Core.Application.Pipelines.Authorization;
using MediatR;
using static Application.Features.Constants.GeneralOperationClaims;


namespace Application.Features.MainFeatures.WebOptions.Queries.WebOptions;
public class GetWebOptionsQuery : IRequest<List<GetWebOptionsResponse>>, IRoleRequest
{
    public string[] Roles => new[] { Admin, SuperAdmin };

    public class GetWebOptionsQueryHandler : IRequestHandler<GetWebOptionsQuery, List<GetWebOptionsResponse>>
    {
        private readonly IWebOptionService _optionService;
        private readonly IMapper _mapper;

        public GetWebOptionsQueryHandler(IWebOptionService optionService, IMapper mapper)
        {
            _optionService = optionService;
            _mapper = mapper;
        }

        public async Task<List<GetWebOptionsResponse>> Handle(GetWebOptionsQuery request, CancellationToken cancellationToken)
        {
            var webOptions = _optionService.GetAll();
            var mappedWebOptions = _mapper.Map<List<GetWebOptionsResponse>>(webOptions);
            return mappedWebOptions;
        }
    }
}
