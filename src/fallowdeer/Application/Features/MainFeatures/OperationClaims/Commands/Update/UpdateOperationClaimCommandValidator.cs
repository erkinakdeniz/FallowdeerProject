using FluentValidation;

namespace Application.Features.MainFeatures.OperationClaims.Commands.Update;

public class UpdateOperationClaimCommandValidator : AbstractValidator<UpdateOperationClaimCommand>
{
    public UpdateOperationClaimCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty().MinimumLength(2).MaximumLength(60).Matches(@"^[A-Za-z\s0-9ğüşıöçİĞÜŞÖÇ]*$").WithMessage("'{PropertyName}' sadece harfler, boşluklar ve rakamlar içermelidir.");
    }
}
