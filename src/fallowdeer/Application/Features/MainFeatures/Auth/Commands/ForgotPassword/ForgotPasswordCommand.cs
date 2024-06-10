using Core.Mailing;
using MediatR;
using MimeKit;

namespace Application.Features.MainFeatures.Auth.Commands.ForgotPassword;
public class ForgotPasswordCommand : IRequest<Unit>
{
    public string Email { get; set; }
    public string Code { get; set; }
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Unit>
    {

        private readonly IMailService _mailService;

        public ForgotPasswordCommandHandler(IMailService mailService)
        {
            _mailService = mailService;
        }

        public async Task<Unit> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var mail = new Mail();
            mail.Subject = "Şifremi Unuttum";
            mail.HtmlBody = request.Code;
            var addresses = new List<MailboxAddress>();
            addresses.Add(new MailboxAddress(request.Email, request.Email));
            mail.ToList = addresses;
            await _mailService.SendEmailAsync(mail);
            return Unit.Task.Result;
        }
    }
}
