using Application.Features.MainFeatures.Users.Commands.Update.Profile;
using Application.Features.MainFeatures.Users.Rules;
using Application.Services.MainServices.AuthService;
using Application.Services.Repositories;
using Core.Application.Pipelines.Authorization;
using Core.Application.Pipelines.Transaction;
using Core.Security.Entities;
using Core.Security.Hashing;
using Core.Security.JWT;
using MediatR;

namespace Application.Features.MainFeatures.Users.Commands.Update.Password;
//Giriş Yapmış kişi kendi şifresini değiştirmek istediğinde
public class UpdateUserPasswordCommand : IRequest<UpdatedUserProfileResponse>, ISecuredAndUserId, ITransactionalRequest
{
    public UpdateUserPasswordDto PasswordDto { get; set; }
    public Guid UserId { get; set; }
    public string IpAddress { get; set; }
    public class UpdateUserPasswordCommandHandler : IRequestHandler<UpdateUserPasswordCommand, UpdatedUserProfileResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly UserBusinessRules _userBusinessRules;
        private readonly IAuthService _authService;

        public UpdateUserPasswordCommandHandler(IUserRepository userRepository, UserBusinessRules userBusinessRules, IAuthService authService)
        {
            _userRepository = userRepository;
            _userBusinessRules = userBusinessRules;
            _authService = authService;
        }

        public async Task<UpdatedUserProfileResponse> Handle(UpdateUserPasswordCommand request, CancellationToken cancellationToken)
        {

            User? user = _userRepository.Get(predicate: u => u.Id == request.UserId);
            await _userBusinessRules.UserShouldBeExistsWhenSelected(user);
            await _userBusinessRules.UserPasswordShouldBeMatched(user: user!, request.PasswordDto.CurrentPassword);
            if (request.PasswordDto.NewPassword != null && !string.IsNullOrWhiteSpace(request.PasswordDto.CurrentPassword))
            {
                HashingHelper.CreatePasswordHash(
                    request.PasswordDto.NewPassword,
                    passwordHash: out byte[] passwordHash,
                    passwordSalt: out byte[] passwordSalt
                );
                user!.PasswordHash = passwordHash;
                user!.PasswordSalt = passwordSalt;
            }
            User updatedUser = await _userRepository.UpdateAsync(user!);

            //UpdatedUserProfileResponse response = _mapper.Map<UpdatedUserProfileResponse>(updatedUser);
            var response = new UpdatedUserProfileResponse();
            AccessToken accessToken = await _authService.CreateAccessToken(user!);
            RefreshToken createdRefreshToken = await _authService.CreateRefreshToken(user!, request.IpAddress);
            RefreshToken addedRefreshToken = await _authService.AddRefreshToken(createdRefreshToken);
            await _authService.DeleteOldRefreshTokens(user.Id);
            response.Expiration = addedRefreshToken.Expires;
            response.Referer = addedRefreshToken.Token;
            response.Token = accessToken.Token;
            return response;
        }
    }
}
