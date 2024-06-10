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

namespace Application.Features.MainFeatures.Auth.Commands.DisableEmailAuthenticator;
public class DisableEmailAuthenticatorCommand : IRequest<Unit>, ISecuredAndUserId, ITransactionalRequest
{
    public Guid UserId { get; set; }
    public class DisableEmailAuthenticatorCommandHandler : IRequestHandler<DisableEmailAuthenticatorCommand, Unit>
    {
        private readonly IUserService _userService;
        private readonly IEmailAuthenticatorRepository _emailAuthenticatorRepository;
        private readonly IMailService _mailService;
        private readonly AuthBusinessRules _authBusinessRules;

        public DisableEmailAuthenticatorCommandHandler(IUserService userService, IEmailAuthenticatorRepository emailAuthenticatorRepository, IMailService mailService, AuthBusinessRules authBusinessRules)
        {
            _userService = userService;
            _emailAuthenticatorRepository = emailAuthenticatorRepository;
            _mailService = mailService;
            _authBusinessRules = authBusinessRules;
        }

        public async Task<Unit> Handle(DisableEmailAuthenticatorCommand request, CancellationToken cancellationToken)
        {
            EmailAuthenticator? emailAuthenticator = await _emailAuthenticatorRepository.GetAsync(x => x.UserId == request.UserId, cancellationToken: cancellationToken);
            await _authBusinessRules.EmailAuthenticatorShouldBeExists(emailAuthenticator);

            User? user = await _userService.GetAsync(x => x.Id == request.UserId, cancellationToken: cancellationToken);
            await _authBusinessRules.UserShouldBeExistsWhenSelected(user);
            var types = new List<AuthenticatorType>();

            foreach (var item in user.AuthenticatorTypes)
                types.Add(item);
            foreach (var item in types)
                if (item == AuthenticatorType.Email)
                    user.AuthenticatorTypes.Remove(item);
            await _userService.UpdateAsync(user);
            await _emailAuthenticatorRepository.DeleteAsync(emailAuthenticator!);
            var toEmailList = new List<MailboxAddress> { new(name: $"{user.FirstName} {user.LastName}", user.Email) };
            _mailService.SendMail(
                new Mail
                {
                    ToList = toEmailList,
                    Subject = "E-Posta Doğrulama Özelliği Kapatıldı | Kodkop Teknoloji",
                    HtmlBody =
                        $"E-Posta Doğrulama özelliğini kapattınız."
                }
            );
            return Unit.Task.Result;
        }
    }
}
