using Application.Features.MainFeatures.Auth.Rules;
using Application.Services.MainServices.AuthenticatorService;
using Application.Services.MainServices.AuthService;
using Application.Services.Repositories;
using Core.Application.Dtos;
using Core.Application.Pipelines.License;
using Core.Security.Entities;
using Core.Security.Enums;
using Core.Security.JWT;
using MediatR;

namespace Application.Features.MainFeatures.Auth.Commands.Login.Login;
public class LoginCommand : IRequest<LoggedResponse>//,ILicenseControl
{
    public UserForLoginDto UserForLoginDto { get; set; }
    public string IpAddress { get; set; }
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoggedResponse>
    {
        private readonly AuthBusinessRules _authBusinessRules;
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticatorService _authenticatorService;

        public LoginCommandHandler(AuthBusinessRules authBusinessRules, IAuthService authService, IUserRepository userRepository, IAuthenticatorService authenticatorService)
        {
            _authBusinessRules = authBusinessRules;
            _authService = authService;
            _userRepository = userRepository;
            _authenticatorService = authenticatorService;
        }

        public async Task<LoggedResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            User? user = _userRepository.Get(u => u.Email == request.UserForLoginDto.Email.Trim());
            await _authBusinessRules.IsThereUserLogin(user);
            await _authBusinessRules.UserPasswordShouldBeMatch(user!.Id, request.UserForLoginDto.Password);
            LoggedResponse loggedResponse = new();
            if (user.AuthenticatorTypes is not null)
                if (user.AuthenticatorTypes.Any(x => x == AuthenticatorType.Email))
                {
                    loggedResponse.RequiredAuthenticatorType = AuthenticatorType.Email;
                    await _authenticatorService.SendAuthenticatorCode(user, AuthenticatorType.Email);
                    return loggedResponse;
                }
                else if (user.AuthenticatorTypes.Any(x => x == AuthenticatorType.Otp))
                {
                    loggedResponse.RequiredAuthenticatorType = AuthenticatorType.Otp;
                    return loggedResponse;
                }

            AccessToken createdAccessToken = await _authService.CreateAccessToken(user);

            Core.Security.Entities.RefreshToken createdRefreshToken = await _authService.CreateRefreshToken(user, request.IpAddress);
            Core.Security.Entities.RefreshToken addedRefreshToken = await _authService.AddRefreshToken(createdRefreshToken);
            await _authService.DeleteOldRefreshTokens(user.Id);

            loggedResponse.AccessToken = createdAccessToken;
            loggedResponse.RefreshToken = addedRefreshToken;
            loggedResponse.RequiredAuthenticatorType = AuthenticatorType.None;
            return loggedResponse;
        }
    }
}
