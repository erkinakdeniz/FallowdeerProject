using Application.Services.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Application.Pipelines.Caching;
using Core.Security.Entities;
using MediatR;
using static Application.Features.Constants.GeneralOperationClaims;

namespace Application.Features.MainFeatures.OperationClaims.Queries.GetAll;
public class GetAllOperationClaimsQuery : IRequest<List<OperationClaimResponse>>, IRoleRequest, ICachableRequest
{
    public bool BypassCache => false;

    public string CacheKey => "GetAllOperationClaimsQuery";

    public string? CacheGroupKey => "Get";

    public TimeSpan? SlidingExpiration => TimeSpan.FromMinutes(5);

    public string[] Roles => new[] { Admin, SuperAdmin };

    public class GetAllOperationClaimsQueryHandler : IRequestHandler<GetAllOperationClaimsQuery, List<OperationClaimResponse>>
    {
        private readonly IOperationClaimRepository _operationClaimRepository;
        private readonly IMapper _mapper;

        public GetAllOperationClaimsQueryHandler(IOperationClaimRepository operationClaimRepository, IMapper mapper)
        {
            _operationClaimRepository = operationClaimRepository;
            _mapper = mapper;
        }

        public async Task<List<OperationClaimResponse>> Handle(GetAllOperationClaimsQuery request, CancellationToken cancellationToken)
        {
            List<OperationClaim> operationClaims = await _operationClaimRepository.QueryAsync(x => !x.Name.Contains("Süper Admin"));
            if (operationClaims == null)
                operationClaims = new();
            var mapped = _mapper.Map<List<OperationClaimResponse>>(operationClaims!);


            return mapped;
        }
    }
}
