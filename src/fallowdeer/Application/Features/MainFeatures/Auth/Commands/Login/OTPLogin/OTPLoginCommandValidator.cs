using FluentValidation;

namespace Application.Features.MainFeatures.Auth.Commands.Login.OTPLogin;
public class OTPLoginCommandValidator : AbstractValidator<OtpLoginCommandDto>
{
    public OTPLoginCommandValidator()
    {
        RuleFor(x => x.AuthenticatorCode).NotNull().NotEmpty();
        RuleFor(x => x.Email).EmailAddress();
    }
}
