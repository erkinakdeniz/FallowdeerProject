using FluentValidation;

namespace Application.Features.MainFeatures.Auth.Commands.Login.EmailLogin;
public class EmailLoginCommandValidator : AbstractValidator<EmailLoginDto>
{
    public EmailLoginCommandValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Code).NotNull().NotEmpty().MaximumLength(10).Matches(@"^[A-Za-z\s0-9ğüşıöçİĞÜŞÖÇ]*$").WithMessage("'{PropertyName}' sadece harfler, boşluklar ve rakamlar içermelidir.");
    }
}
