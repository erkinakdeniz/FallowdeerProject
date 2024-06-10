using Application.Features.MainFeatures.Auth.Rules;
using Application.Services.MainServices.AuthenticatorService;
using Application.Services.MainServices.UsersService;
using Application.Services.Repositories;
using Core.Application.Pipelines.Authorization;
using Core.Security.Entities;
using Core.Security.Enums;
using MediatR;

namespace Application.Features.MainFeatures.Auth.Commands.VerifyOtpAuthenticator;

public class VerifyOtpAuthenticatorCommand : IRequest, ISecuredAndUserId
{
    public Guid UserId { get; set; }
    public string ActivationCode { get; set; }

    public VerifyOtpAuthenticatorCommand()
    {
        ActivationCode = string.Empty;
    }

    public VerifyOtpAuthenticatorCommand(Guid userId, string activationCode)
    {
        UserId = userId;
        ActivationCode = activationCode;
    }

    public class VerifyOtpAuthenticatorCommandHandler : IRequestHandler<VerifyOtpAuthenticatorCommand>
    {
        private readonly AuthBusinessRules _authBusinessRules;
        private readonly IAuthenticatorService _authenticatorService;
        private readonly IOtpAuthenticatorRepository _otpAuthenticatorRepository;
        private readonly IUserService _userService;

        public VerifyOtpAuthenticatorCommandHandler(
            IOtpAuthenticatorRepository otpAuthenticatorRepository,
            AuthBusinessRules authBusinessRules,
            IUserService userService,
            IAuthenticatorService authenticatorService
        )
        {
            _otpAuthenticatorRepository = otpAuthenticatorRepository;
            _authBusinessRules = authBusinessRules;
            _userService = userService;
            _authenticatorService = authenticatorService;
        }

        public async Task Handle(VerifyOtpAuthenticatorCommand request, CancellationToken cancellationToken)
        {
            OtpAuthenticator? otpAuthenticator = await _otpAuthenticatorRepository.GetAsync(
                predicate: e => e.UserId == request.UserId,
                cancellationToken: cancellationToken
            );
            await _authBusinessRules.OtpAuthenticatorShouldBeExists(otpAuthenticator);

            User? user = await _userService.GetAsync(predicate: u => u.Id == request.UserId, cancellationToken: cancellationToken);
            await _authBusinessRules.UserShouldBeExistsWhenSelected(user);

            otpAuthenticator!.IsVerified = true;
            user!.AuthenticatorTypes.Add(AuthenticatorType.Otp);

            await _authenticatorService.VerifyAuthenticatorCode(user, request.ActivationCode, AuthenticatorType.Otp);

            await _otpAuthenticatorRepository.UpdateAsync(otpAuthenticator);
            await _userService.UpdateAsync(user);
        }
    }
}
