using Application.Features.MainFeatures.Users.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Application.Pipelines.Transaction;
using Core.Security.Entities;
using Core.Security.Hashing;
using MediatR;
using static Application.Features.Constants.GeneralOperationClaims;

namespace Application.Features.MainFeatures.Users.Commands.Update.ChangePassword;
//Admin panelinden herhangi bir kullanıcının şifresi değiştirildiğinde
public class UpdateUserChangePasswordCommand : IRequest<Unit>, IRoleRequest, ITransactionalRequest
{
    public Guid Id { get; set; }
    public string Password { get; set; }

    public string[] Roles => new[] { Admin, SuperAdmin };

    public class UpdateUserChangePasswordCommandHandler : IRequestHandler<UpdateUserChangePasswordCommand, Unit>
    {
        private readonly IUserRepository _userRepository;

        private readonly UserBusinessRules _userBusinessRules;

        public UpdateUserChangePasswordCommandHandler(IUserRepository userRepository, UserBusinessRules userBusinessRules)
        {
            _userRepository = userRepository;

            _userBusinessRules = userBusinessRules;
        }

        public async Task<Unit> Handle(UpdateUserChangePasswordCommand request, CancellationToken cancellationToken)
        {

            User? user = _userRepository.Get(predicate: u => u.Id == request.Id);
            await _userBusinessRules.UserShouldBeExistsWhenSelected(user);


            HashingHelper.CreatePasswordHash(
                request.Password,
                passwordHash: out byte[] passwordHash,
                passwordSalt: out byte[] passwordSalt
            );
            user!.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            await _userRepository.UpdateAsync(user);
            return Unit.Task.Result;

        }
    }
}
