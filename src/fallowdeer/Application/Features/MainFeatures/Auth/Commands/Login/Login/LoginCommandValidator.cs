using FluentValidation;

namespace Application.Features.MainFeatures.Auth.Commands.Login.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(c => c.UserForLoginDto.Email).EmailAddress();
        RuleFor(c => c.UserForLoginDto.Password).NotEmpty().NotNull();
    }
}
