using Application.Services.MainServices.UsersService;
using Core.Application.Pipelines.Transaction;
using Core.CrossCuttingConcerns;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Core.CrossCuttingConcerns.Extensions;
using Core.Security.Entities;
using Core.Security.Hashing;
using Core.Security.JWT;
using MediatR;

namespace Application.Features.MainFeatures.Auth.Commands.ResetPassword;
public class ResetPasswordCommand : IRequest, ITransactionalRequest
{
    public string Token { get; set; }
    public string Password { get; set; }
    public string PasswordConfirm { get; set; }
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
    {

        private readonly ITokenHelper _tokenHelper;
        private readonly IUserService _userService;


        public ResetPasswordCommandHandler(ITokenHelper tokenHelper, IUserService userService)
        {
            _tokenHelper = tokenHelper;
            _userService = userService;
        }

        public async Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var jwt = _tokenHelper.TokenVerify(request.Token);
                var userId = jwt.Payload.Where(x => x.Key.Equals(MyClaimTypes.ID)).Select(x => x.Value).FirstOrDefault()?.ToString();
                if (userId != null)
                {
                    User? user = await _userService.GetAsync(x => x.Id == Guid.Parse(userId), cancellationToken: cancellationToken);
                    if (user != null)
                    {
                        HashingHelper.CreatePasswordHash(
                                    request.Password,
                                    passwordHash: out byte[] passwordHash,
                                    passwordSalt: out byte[] passwordSalt
                                    );
                        user.PasswordHash = passwordHash;
                        user.PasswordSalt = passwordSalt;
                        await _userService.UpdateAsync(user);

                    }
                }
            }
            catch (Exception)
            {
                throw new AuthorizationException(SecurityMessages.AuthenticatedException);
            }
        }
    }
}
