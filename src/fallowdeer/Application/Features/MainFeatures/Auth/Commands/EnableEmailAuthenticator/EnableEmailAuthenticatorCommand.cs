using Application.Features.MainFeatures.Auth.Rules;
using Application.Services.MainServices.AuthenticatorService;
using Application.Services.MainServices.UsersService;
using Application.Services.Repositories;
using Core.Application.Pipelines.Authorization;
using Core.Application.Pipelines.Transaction;
using Core.Mailing;
using Core.Security.Entities;
using Core.Security.Enums;
using MediatR;
using MimeKit;
using System.Web;

namespace Application.Features.MainFeatures.Auth.Commands.EnableEmailAuthenticator;

public class EnableEmailAuthenticatorCommand : IRequest<Unit>, ISecuredAndUserId, ITransactionalRequest
{
    public Guid UserId { get; set; }

    public class EnableEmailAuthenticatorCommandHandler : IRequestHandler<EnableEmailAuthenticatorCommand, Unit>
    {
        private readonly AuthBusinessRules _authBusinessRules;
        private readonly IAuthenticatorService _authenticatorService;
        private readonly IEmailAuthenticatorRepository _emailAuthenticatorRepository;
        private readonly IMailService _mailService;
        private readonly IUserService _userService;


        public EnableEmailAuthenticatorCommandHandler(
            IUserService userService,
            IEmailAuthenticatorRepository emailAuthenticatorRepository,
            IMailService mailService,
            AuthBusinessRules authBusinessRules,
            IAuthenticatorService authenticatorService
        )
        {
            _userService = userService;
            _emailAuthenticatorRepository = emailAuthenticatorRepository;
            _mailService = mailService;
            _authBusinessRules = authBusinessRules;
            _authenticatorService = authenticatorService;
        }

        public async Task<Unit> Handle(EnableEmailAuthenticatorCommand request, CancellationToken cancellationToken)
        {
            User? user = await _userService.GetAsync(predicate: u => u.Id == request.UserId, cancellationToken: cancellationToken);
            await _authBusinessRules.UserShouldBeExistsWhenSelected(user);
            await _authBusinessRules.UserShouldNotBeHaveEmailAuthenticator(user!);



            EmailAuthenticator emailAuthenticator = await _authenticatorService.CreateEmailAuthenticator(user);
            EmailAuthenticator addedEmailAuthenticator = await _emailAuthenticatorRepository.AddAsync(emailAuthenticator);

            var toEmailList = new List<MailboxAddress> { new(name: $"{user.FirstName} {user.LastName}", user.Email) };

            //_mailService.SendMail(
            //    new Mail
            //    {
            //        ToList = toEmailList,
            //        Subject = "E-Posta Adresini Doğrula | Kodkop Teknoloji",
            //        HtmlBody =
            //            $"E-Posta adresinizi doğrulamak için linke tıklayınız: {request.VerifyEmailUrlPrefix}?ActivationKey={HttpUtility.UrlEncode(addedEmailAuthenticator.ActivationKey)}"
            //    }
            //);
            _mailService.SendMail(
                new Mail
                {
                    ToList = toEmailList,
                    Subject = "E-Posta Adresini Doğrula | Kodkop Teknoloji",
                    HtmlBody =
                        $"E-Posta adresinizi doğrulama kodunuz: {addedEmailAuthenticator.ActivationKey}"
                }
            );
            return Unit.Task.Result;

        }
    }
}
