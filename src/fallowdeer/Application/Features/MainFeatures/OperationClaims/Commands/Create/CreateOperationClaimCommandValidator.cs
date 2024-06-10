using FluentValidation;

namespace Application.Features.MainFeatures.OperationClaims.Commands.Create;

public class CreateOperationClaimCommandValidator : AbstractValidator<CreateOperationClaimCommand>
{
    public CreateOperationClaimCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty().MinimumLength(2).MaximumLength(60).Matches(@"^[A-Za-z\s0-9ğüşıöçİĞÜŞÖÇ]*$").WithMessage("'{PropertyName}' sadece harfler, boşluklar ve rakamlar içermelidir.");
    }
}
