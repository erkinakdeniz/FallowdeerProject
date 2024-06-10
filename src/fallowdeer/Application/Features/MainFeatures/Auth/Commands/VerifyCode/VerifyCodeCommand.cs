using Application.Features.MainFeatures.Auth.Constants;
using Application.Services.MainServices.AuthService;
using Application.Services.MainServices.UsersService;
using Application.Services.Repositories;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Core.Security.Entities;
using Core.Security.JWT;
using MediatR;

namespace Application.Features.MainFeatures.Auth.Commands.VerifyCode;
public class VerifyCodeCommand : IRequest<VerifyCodeResponse>
{
    public string Email { get; set; }
    public string Code { get; set; }
    public class VerifyCodeCommandHandler : IRequestHandler<VerifyCodeCommand, VerifyCodeResponse>
    {
        private readonly IUnicodeRepository _unicodeRepository;
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public VerifyCodeCommandHandler(IUnicodeRepository unicodeRepository, IUserService userService, IAuthService authService)
        {
            _unicodeRepository = unicodeRepository;
            _userService = userService;
            _authService = authService;
        }

        public async Task<VerifyCodeResponse> Handle(VerifyCodeCommand request, CancellationToken cancellationToken)
        {
            var unicodes = _unicodeRepository.Query(x => x.ExpiredDate < DateTime.Now).ToList();
            foreach (var item in unicodes)
                await _unicodeRepository.DeleteAsync(item);
            Unicode? unicode = await _unicodeRepository.GetAsync(x => x.Email == request.Email && x.Code == request.Code, cancellationToken: cancellationToken);
            if (unicode != null)
            {
                User? user = await _userService.GetAsync(x => x.Email == request.Email, cancellationToken: cancellationToken);
                if (user != null)
                {
                    AccessToken createdAccessToken = await _authService.CreateAccessToken(user);
                    await _unicodeRepository.DeleteAsync(unicode);
                    return new VerifyCodeResponse() { Token = createdAccessToken.Token };
                }
                else
                    throw new BusinessException(AuthMessages.InvalidCode);

            }
            else
                throw new BusinessException(AuthMessages.InvalidCode);
        }
    }
}
