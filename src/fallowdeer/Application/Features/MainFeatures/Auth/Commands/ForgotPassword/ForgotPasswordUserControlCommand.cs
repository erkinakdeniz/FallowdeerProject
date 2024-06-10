using Application.Services.MainServices.UsersService;
using Application.Services.Repositories;
using Core.Helper;
using MediatR;
using static Application.Features.MainFeatures.Auth.Commands.ForgotPassword.ForgotPasswordUserControlCommand;

namespace Application.Features.MainFeatures.Auth.Commands.ForgotPassword;
public class ForgotPasswordUserControlCommand : IRequest<ForgotPasswordUserControlCommandResponse>
{
    public string Email { get; set; }
    public class ForgotPasswordUserControlCommandHandler : IRequestHandler<ForgotPasswordUserControlCommand, ForgotPasswordUserControlCommandResponse>
    {
        private readonly IUserService _userService;
        private readonly IUnicodeRepository _unicodeRepository;

        public ForgotPasswordUserControlCommandHandler(IUserService userService, IUnicodeRepository unicodeRepository)
        {
            _userService = userService;
            _unicodeRepository = unicodeRepository;
        }

        public async Task<ForgotPasswordUserControlCommandResponse> Handle(ForgotPasswordUserControlCommand request, CancellationToken cancellationToken)
        {
            var user = await _userService.GetAsync(x => x.Email == request.Email, cancellationToken: cancellationToken);
            if (user is not null)
            {
                string code = RandomGenerator.CreateNumberRandom().ToString();
                await _unicodeRepository.AddAsync(new Core.Security.Entities.Unicode()
                {
                    Code = code,
                    Email = request.Email
                });
                return new ForgotPasswordUserControlCommandResponse() { Email = request.Email, Code = code };
            }
            else
                return default;

        }
    }
    public class ForgotPasswordUserControlCommandResponse
    {
        public string Email { get; set; }
        public string Code { get; set; }
    }
}
