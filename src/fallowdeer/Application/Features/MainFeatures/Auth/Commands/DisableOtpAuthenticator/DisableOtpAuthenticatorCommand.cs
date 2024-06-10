using Application.Features.MainFeatures.Auth.Rules;
using Application.Services.MainServices.UsersService;
using Application.Services.Repositories;
using Core.Application.Pipelines.Authorization;
using Core.Application.Pipelines.Transaction;
using Core.Mailing;
using Core.Security.Entities;
using Core.Security.Enums;
using MediatR;
using MimeKit;

namespace Application.Features.MainFeatures.Auth.Commands.DisableOtpAuthenticator;
public class DisableOtpAuthenticatorCommand : IRequest<Unit>, ISecuredAndUserId, ITransactionalRequest
{
    public Guid UserId { get; set; }
    public class DisableOtpAuthenticatorCommandHandler : IRequestHandler<DisableOtpAuthenticatorCommand, Unit>
    {
        private readonly IUserService _userService;
        private readonly IOtpAuthenticatorRepository _authenticatorRepository;
        private readonly IMailService _mailService;
        private readonly AuthBusinessRules _authBusinessRules;

        public DisableOtpAuthenticatorCommandHandler(IUserService userService, IOtpAuthenticatorRepository authenticatorRepository, IMailService mailService, AuthBusinessRules authBusinessRules)
        {
            _userService = userService;
            _authenticatorRepository = authenticatorRepository;
            _mailService = mailService;
            _authBusinessRules = authBusinessRules;
        }

        public async Task<Unit> Handle(DisableOtpAuthenticatorCommand request, CancellationToken cancellationToken)
        {
            OtpAuthenticator? otpAuthenticator = await _authenticatorRepository.GetAsync(x => x.UserId == request.UserId, cancellationToken: cancellationToken);
            await _authBusinessRules.OtpAuthenticatorShouldBeExists(otpAuthenticator);

            User? user = await _userService.GetAsync(x => x.Id == request.UserId, cancellationToken: cancellationToken);
            await _authBusinessRules.UserShouldBeExistsWhenSelected(user);
            var types = new List<AuthenticatorType>();

            foreach (var item in user.AuthenticatorTypes)
                types.Add(item);
            foreach (var item in types)
                if (item == AuthenticatorType.Otp)
                    user.AuthenticatorTypes.Remove(item);
            await _userService.UpdateAsync(user);
            await _authenticatorRepository.DeleteAsync(otpAuthenticator);
            var toEmailList = new List<MailboxAddress> { new(name: $"{user.FirstName} {user.LastName}", user.Email) };
            _mailService.SendMail(
                new Mail
                {
                    ToList = toEmailList,
                    Subject = "Tek Kullanımlık Şifre Kapatıldı | Kodkop Teknoloji",
                    HtmlBody =
                        $"Tek kullanımlık şifre özelliğini kapattınız."
                }
            );
            return Unit.Task.Result;
        }
    }
}
