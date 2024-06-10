using Application.Features.MainFeatures.Auth.Rules;
using Application.Features.MainFeatures.WebOptions.Rules;
using Application.Services.MainServices.AuthService;
using Application.Services.Repositories;
using Core.Application.Dtos;
using Core.Application.Pipelines.Transaction;
using Core.Security.Entities;
using Core.Security.Enums;
using Core.Security.Hashing;
using Core.Security.JWT;
using MediatR;

namespace Application.Features.MainFeatures.Auth.Commands.Register;

public class RegisterCommand : IRequest<RegisteredResponse>, ITransactionalRequest
{
    public UserForRegisterDto UserForRegisterDto { get; set; }
    public string IpAddress { get; set; }

    public RegisterCommand()
    {
        UserForRegisterDto = null!;
        IpAddress = string.Empty;
    }

    public RegisterCommand(UserForRegisterDto userForRegisterDto, string ipAddress)
    {
        UserForRegisterDto = userForRegisterDto;
        IpAddress = ipAddress;
    }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisteredResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;
        private readonly AuthBusinessRules _authBusinessRules;
        private readonly WebOptionBusinessRules _webOptionBusinessRules;

        public RegisterCommandHandler(IUserRepository userRepository, IAuthService authService, AuthBusinessRules authBusinessRules, WebOptionBusinessRules webOptionBusinessRules)
        {
            _userRepository = userRepository;
            _authService = authService;
            _authBusinessRules = authBusinessRules;
            _webOptionBusinessRules = webOptionBusinessRules;
        }

        public async Task<RegisteredResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            //Kullanıcıların dışarıdan kayıt olması. Yetki olarak kullanıcı atanır.
            await _webOptionBusinessRules.IsActiveExternalRegistration();
            await _authBusinessRules.UserEmailShouldBeNotExists(request.UserForRegisterDto.Email);

            HashingHelper.CreatePasswordHash(
                request.UserForRegisterDto.Password,
                passwordHash: out byte[] passwordHash,
                passwordSalt: out byte[] passwordSalt
            );
            User newUser =
                new()
                {
                    Email = request.UserForRegisterDto.Email.Trim(),
                    FirstName = request.UserForRegisterDto.FirstName.Trim(),
                    LastName = request.UserForRegisterDto.LastName.Trim(),
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Status = true,
                    AuthenticatorTypes = new List<AuthenticatorType>(),
                    UserOperationClaims = new List<UserOperationClaim>()

                };
            User createdUser = _userRepository.Add(newUser);
            var userOperationClaim = new UserOperationClaim(createdUser.Id, 3);
            createdUser.UserOperationClaims.Add(userOperationClaim);
            _userRepository.Update(createdUser);

            AccessToken createdAccessToken = await _authService.CreateAccessToken(createdUser);

            Core.Security.Entities.RefreshToken createdRefreshToken = await _authService.CreateRefreshToken(createdUser, request.IpAddress);
            Core.Security.Entities.RefreshToken addedRefreshToken = await _authService.AddRefreshToken(createdRefreshToken);

            RegisteredResponse registeredResponse = new() { AccessToken = createdAccessToken, RefreshToken = addedRefreshToken };
            return registeredResponse;
        }
    }
}
