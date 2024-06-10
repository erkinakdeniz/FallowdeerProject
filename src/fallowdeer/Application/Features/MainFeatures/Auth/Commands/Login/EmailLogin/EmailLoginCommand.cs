using Application.Features.MainFeatures.Auth.Rules;
using Application.Services.MainServices.AuthenticatorService;
using Application.Services.MainServices.AuthService;
using Application.Services.Repositories;
using Core.Security.Entities;
using Core.Security.Enums;
using Core.Security.JWT;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace Application.Features.MainFeatures.Auth.Commands.Login.EmailLogin;

public class EmailLoginCommand : IRequest<LoggedResponse>
{
    public string Email { get; set; }
    public string Code { get; set; }
    public string IpAddress { get; set; }

    public EmailLoginCommand()
    {
        Email = null!;
        IpAddress = string.Empty;
    }

    public EmailLoginCommand(string email, string code, string ıpAddress)
    {
        Email = email;
        Code = code;
        IpAddress = ıpAddress;
    }

    public class EmailLoginCommandHandler : IRequestHandler<EmailLoginCommand, LoggedResponse>
    {
        private readonly AuthBusinessRules _authBusinessRules;
        private readonly IAuthenticatorService _authenticatorService;
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;


        public EmailLoginCommandHandler(
            IUserRepository userRepository,
            IAuthService authService,
            AuthBusinessRules authBusinessRules,
            IAuthenticatorService authenticatorService)
        {
            _userRepository = userRepository;
            _authService = authService;
            _authBusinessRules = authBusinessRules;
            _authenticatorService = authenticatorService;
        }

        public async Task<LoggedResponse> Handle(EmailLoginCommand request, CancellationToken cancellationToken)
        {


            User? user = _userRepository.Get(
                predicate: u => u.Email == request.Email.Trim()
            );
            await _authBusinessRules.IsThereUserLogin(user);

            LoggedResponse loggedResponse = new();

            if (user.AuthenticatorTypes.Any(x => x == AuthenticatorType.Email))
            {
                if (request.Code.IsNullOrEmpty())
                {
                    await _authenticatorService.SendAuthenticatorCode(user, AuthenticatorType.Email);
                    loggedResponse.RequiredAuthenticatorType = AuthenticatorType.Email;
                    return loggedResponse;
                }
                await _authenticatorService.VerifyAuthenticatorCode(user, request.Code, AuthenticatorType.Email);
            }
            if (user.AuthenticatorTypes.Any(x => x == AuthenticatorType.Otp))
            {
                loggedResponse.RequiredAuthenticatorType = AuthenticatorType.Otp;
                return loggedResponse;
            }
            else
                loggedResponse.RequiredAuthenticatorType = AuthenticatorType.None;


            AccessToken createdAccessToken = await _authService.CreateAccessToken(user);

            Core.Security.Entities.RefreshToken createdRefreshToken = await _authService.CreateRefreshToken(user, request.IpAddress);
            Core.Security.Entities.RefreshToken addedRefreshToken = await _authService.AddRefreshToken(createdRefreshToken);
            await _authService.DeleteOldRefreshTokens(user.Id);

            loggedResponse.AccessToken = createdAccessToken;
            loggedResponse.RefreshToken = addedRefreshToken;

            return loggedResponse;
        }
    }
}
