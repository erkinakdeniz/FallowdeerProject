using FluentValidation;

namespace Application.Features.MainFeatures.WebOptions.Commands.Update.UpdateSlider;
public class UpdateSliderRequestValidator:AbstractValidator<UpdateSliderRequest>
{
    public UpdateSliderRequestValidator()
    {
           RuleFor(x=>x.Title).MaximumLength(60).Matches(@"^[A-Za-z\s0-9ğüşıöçİĞÜŞÖÇ]*$").WithMessage("'{PropertyName}' sadece harfler, boşluklar ve rakamlar içermelidir.");
           RuleFor(x=>x.Description).MaximumLength(60).Matches(@"^[A-Za-z\s0-9ğüşıöçİĞÜŞÖÇ]*$").WithMessage("'{PropertyName}' sadece harfler, boşluklar ve rakamlar içermelidir.");
           RuleFor(x => x.Url).MaximumLength(2083);
    }
}
