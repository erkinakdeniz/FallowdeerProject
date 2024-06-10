using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.MainFeatures.WebOptions.Commands.Update.UpdateSlider;
public class UpdateSliderCommandValidator:AbstractValidator<UpdateSliderCommand>
{
    public UpdateSliderCommandValidator()
    {
        RuleFor(x => x.CategoryId).NotEmpty().NotEmpty();
        RuleForEach(x => x.Slides).SetValidator(new UpdateSliderRequestValidator());
    }
}
