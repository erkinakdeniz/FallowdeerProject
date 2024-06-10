using Application.Features.MainFeatures.Auth.Constants;
using Application.Features.MainFeatures.Auth.Rules;
using Application.Services.MainServices.AuthenticatorService;
using Application.Services.MainServices.AuthService;
using Application.Services.Repositories;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Core.Security.Entities;
using Core.Security.Enums;
using Core.Security.JWT;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace Application.Features.MainFeatures.Auth.Commands.Login.OTPLogin;
public class OTPLoginCommand : IRequest<LoggedResponse>
{
    public string AuthenticatorCode { get; set; }
    public string Email { get; set; }
    public string IpAddress { get; set; }
    public class OTPLoginCommandHandler : IRequestHandler<OTPLoginCommand, LoggedResponse>
    {
        private readonly AuthBusinessRules _authBusinessRules;
        private readonly IAuthenticatorService _authenticatorService;
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;

        public OTPLoginCommandHandler(AuthBusinessRules authBusinessRules, IAuthenticatorService authenticatorService, IAuthService authService, IUserRepository userRepository)
        {
            _authBusinessRules = authBusinessRules;
            _authenticatorService = authenticatorService;
            _authService = authService;
            _userRepository = userRepository;
        }

        public async Task<LoggedResponse> Handle(OTPLoginCommand request, CancellationToken cancellationToken)
        {
            User? user = _userRepository.Get(
                predicate: u => u.Email == request.Email.Trim()
            );
            await _authBusinessRules.UserShouldBeExistsWhenSelected(user);

            LoggedResponse loggedResponse = new();
            if (user.EmailAuthenticators is not null)
                if (user.EmailAuthenticators.Any(x => x.IsVerified == true && !x.ActivationKey.IsNullOrEmpty()))
                {
                    loggedResponse.RequiredAuthenticatorType = AuthenticatorType.Email;
                    return loggedResponse;
                }


            if (user.AuthenticatorTypes.Any(x => x == AuthenticatorType.Otp))
                if (request.AuthenticatorCode is not null)
                {
                    await _authenticatorService.VerifyAuthenticatorCode(user, request.AuthenticatorCode, AuthenticatorType.Otp);

                    AccessToken createdAccessToken = await _authService.CreateAccessToken(user);

                    Core.Security.Entities.RefreshToken createdRefreshToken = await _authService.CreateRefreshToken(user, request.IpAddress);
                    Core.Security.Entities.RefreshToken addedRefreshToken = await _authService.AddRefreshToken(createdRefreshToken);
                    await _authService.DeleteOldRefreshTokens(user.Id);

                    loggedResponse.AccessToken = createdAccessToken;
                    loggedResponse.RefreshToken = addedRefreshToken;
                    return loggedResponse;
                }
                else
                    throw new BusinessException(AuthMessages.AuthenticatorCodeNotFound);
            else
                throw new BusinessException(AuthMessages.OtpAuthenticatorDontExists);
        }
    }
}
