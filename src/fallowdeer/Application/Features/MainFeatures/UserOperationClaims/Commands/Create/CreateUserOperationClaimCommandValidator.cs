using FluentValidation;

namespace Application.Features.MainFeatures.UserOperationClaims.Commands.Create;

public class CreateUserOperationClaimCommandValidator : AbstractValidator<CreateUserOperationClaimCommand>
{
    public CreateUserOperationClaimCommandValidator()
    {
        RuleFor(c => c.UserId).NotNull().NotEmpty();
    }
}
