using Application.Features.MainFeatures.Users.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Security.Entities;
using MediatR;
using static Application.Features.Constants.GeneralOperationClaims;

namespace Application.Features.MainFeatures.Users.Queries.GetById;

public class GetByIdUserQuery : IRequest<GetByIdUserResponse>, IRoleRequest
{
    public Guid Id { get; set; }

    public string[] Roles => new[] { Admin, SuperAdmin,Editor,Constants.GeneralOperationClaims.User };

    public class GetByIdUserQueryHandler : IRequestHandler<GetByIdUserQuery, GetByIdUserResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly UserBusinessRules _userBusinessRules;

        public GetByIdUserQueryHandler(IUserRepository userRepository, IMapper mapper, UserBusinessRules userBusinessRules)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _userBusinessRules = userBusinessRules;
        }

        public async Task<GetByIdUserResponse> Handle(GetByIdUserQuery request, CancellationToken cancellationToken)
        {
            User? user = _userRepository.Get(predicate: b => b.Id == request.Id);
            await _userBusinessRules.UserShouldBeExistsWhenSelected(user);

            GetByIdUserResponse response = _mapper.Map<GetByIdUserResponse>(user);
            return response;
        }
    }
}
