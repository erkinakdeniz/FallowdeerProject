using Application.Features.MainFeatures.Users.Rules;
using Application.Services.MainServices.AuthService;
using Application.Services.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Application.Pipelines.Transaction;
using Core.Security.Entities;
using Core.Security.Hashing;
using Core.Security.JWT;
using MediatR;

namespace Application.Features.MainFeatures.Users.Commands.Update.Profile;

public class UpdateUserProfileCommand : IRequest<UpdatedUserProfileResponse>, ISecuredAndUserId, ITransactionalRequest
{
    public Guid UserId { get; set; }
    public UpdateUserProfileDto User { get; set; }
    public string IpAddress { get; set; }


    public class UpdateUserFromAuthCommandHandler : IRequestHandler<UpdateUserProfileCommand, UpdatedUserProfileResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly UserBusinessRules _userBusinessRules;
        private readonly IAuthService _authService;

        public UpdateUserFromAuthCommandHandler(IUserRepository userRepository, UserBusinessRules userBusinessRules, IAuthService authService)
        {
            _userRepository = userRepository;
            _userBusinessRules = userBusinessRules;
            _authService = authService;
        }

        public async Task<UpdatedUserProfileResponse> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            User? user = _userRepository.Get(predicate: u => u.Id == request.UserId);
            await _userBusinessRules.UserShouldBeExistsWhenSelected(user);
            await _userBusinessRules.UserPasswordShouldBeMatched(user: user!, request.User.CurrentPassword);
            await _userBusinessRules.UserEmailShouldNotExistsWhenUpdate(user!.Id, request.User.Email);

            user.FirstName = request.User.FirstName;
            user.LastName = request.User.LastName;
            user.Email = request.User.Email;
            User updatedUser = await _userRepository.UpdateAsync(user!);
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
