using FluentValidation;

namespace Application.Features.MainFeatures.Auth.Commands.RefreshToken;
public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.RefreshToken).NotNull().NotEmpty().MaximumLength(35);
    }
}
