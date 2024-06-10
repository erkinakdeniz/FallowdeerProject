using FluentValidation;

namespace Application.Features.MainFeatures.WebOptions.Commands.Update;
public class UpdateWebOptionCommandValidator : AbstractValidator<UpdateWebOptionCommand>
{
    public UpdateWebOptionCommandValidator()
    {
        RuleFor(x => x.Title).MinimumLength(3).MaximumLength(50);
        RuleFor(x => x.Alias).MinimumLength(3).MaximumLength(100);
        RuleFor(x => x.InputType).MinimumLength(1).MaximumLength(100);
    }
}
