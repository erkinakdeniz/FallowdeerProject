using FluentValidation;

namespace Application.Features.MainFeatures.WebOptions.Commands.Create.CreateSlider;
public class CreateSliderCommandValidator:AbstractValidator<CreateSliderCommand>
{
    public CreateSliderCommandValidator()
    {
        RuleFor(x => x.CategoryId).NotEmpty().NotEmpty();
        RuleForEach(x => x.Slides).SetValidator(new CreateSliderRequestValidator());
    }
}
