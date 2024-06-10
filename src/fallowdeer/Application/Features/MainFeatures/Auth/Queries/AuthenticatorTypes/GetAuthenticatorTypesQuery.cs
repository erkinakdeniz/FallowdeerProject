using Application.Features.MainFeatures.Auth.Rules;
using Application.Services.MainServices.AuthenticatorService;
using Application.Services.MainServices.UsersService;
using Core.Application.Pipelines.Authorization;
using Core.Security.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.MainFeatures.Auth.Queries.AuthenticatorTypes;
public class GetAuthenticatorTypesQuery : IRequest<GetAuthenticatorTypesQueryResponse>, ISecuredAndUserId
{
    public Guid UserId { get; set; }
    public class GetAuthenticatorTypesQueryHandler : IRequestHandler<GetAuthenticatorTypesQuery, GetAuthenticatorTypesQueryResponse>
    {
        private readonly IUserService _userService;
        private readonly AuthBusinessRules _authBusinessRules;
        private readonly IAuthenticatorService _authenticatorService;

        public GetAuthenticatorTypesQueryHandler(IUserService userService, AuthBusinessRules authBusinessRules, IAuthenticatorService authenticatorService)
        {
            _userService = userService;
            _authBusinessRules = authBusinessRules;
            _authenticatorService = authenticatorService;
        }

        public async Task<GetAuthenticatorTypesQueryResponse> Handle(GetAuthenticatorTypesQuery request, CancellationToken cancellationToken)
        {
            User? user = await _userService.GetAsync(x => x.Id == request.UserId, include: x => x.Include(x => x.EmailAuthenticators).Include(x => x.OtpAuthenticators), cancellationToken: cancellationToken);
            await _authBusinessRules.UserShouldBeExistsWhenSelected(user);
            var data = user.OtpAuthenticators.FirstOrDefault();
            string otpSecretKey = "";
            string otpImageBase64 = "";
            if (data is not null)
            {
                if (data.SecretKey is not null)
                {
                    byte[] otpSecretKeyByte = data.SecretKey;
                    otpSecretKey = await _authenticatorService.ConvertSecretKeyToString(otpSecretKeyByte);
                }
                if (data.ImageBase64 is not null)
                    otpImageBase64 = user.OtpAuthenticators.FirstOrDefault().ImageBase64;
            }
            return new GetAuthenticatorTypesQueryResponse()
            {
                EmailAuthenticator = new GetAuthenticatorTypesQueryResponse.EmailAuthenticatorResponse()
                {
                    Email = user.Email,
                    IsOpen = user.EmailAuthenticators.Count > 0,
                    IsVerify = user.EmailAuthenticators.Any(x => x.IsVerified == true)
                },
                OtpAuthenticator = new GetAuthenticatorTypesQueryResponse.OtpAuthenticatorResponse()
                {
                    IsOpen = user.OtpAuthenticators.Count > 0,
                    IsVerify = user.OtpAuthenticators.Any(x => x.IsVerified == true),
                    OtpImageBase64 = otpImageBase64,
                    OtpSecretKey = otpSecretKey
                }

            };
        }
    }
}
