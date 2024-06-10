using Application.Services.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Security.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Application.Features.Constants.GeneralOperationClaims;

namespace Application.Features.MainFeatures.UserOperationClaims.Queries.GetByUserId;
public class GetByUserIdUserOperationClaimsQuery : IRequest<List<GetByUserIdUserOperationClaimResponse>>, IRoleRequest
{
    public Guid UserId { get; set; }
    public string[] Roles => new[] { Admin, SuperAdmin };
    public class GetByUserIdUserOperationClaimsQueryHandler : IRequestHandler<GetByUserIdUserOperationClaimsQuery, List<GetByUserIdUserOperationClaimResponse>>
    {
        private readonly IUserOperationClaimRepository _userOperationClaimRepository;
        private readonly IMapper _mapper;

        public GetByUserIdUserOperationClaimsQueryHandler(IUserOperationClaimRepository userOperationClaimRepository, IMapper mapper)
        {
            _userOperationClaimRepository = userOperationClaimRepository;
            _mapper = mapper;
        }

        public async Task<List<GetByUserIdUserOperationClaimResponse>> Handle(GetByUserIdUserOperationClaimsQuery request, CancellationToken cancellationToken)
        {
            var userOperationClaim = _userOperationClaimRepository.Query(x => x.UserId == request.UserId, include: x => x.Include(x => x.OperationClaim)).ToList();
            List<GetByUserIdUserOperationClaimResponse> userOperationClaimDto = new();
            if (userOperationClaim != null)
                userOperationClaimDto = _mapper.Map<List<GetByUserIdUserOperationClaimResponse>>(userOperationClaim);
            return userOperationClaimDto;


        }
    }
}
