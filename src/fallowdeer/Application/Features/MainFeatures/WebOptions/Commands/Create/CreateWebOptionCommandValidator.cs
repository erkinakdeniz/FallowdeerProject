using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.MainFeatures.WebOptions.Commands.Create;
public class CreateWebOptionCommandValidator : AbstractValidator<CreateWebOptionCommand>
{
    public CreateWebOptionCommandValidator()
    {
        RuleFor(x => x.Title).MinimumLength(3).MaximumLength(50);
        RuleFor(x => x.Alias).MinimumLength(3).MaximumLength(100);
        RuleFor(x => x.InputType).MinimumLength(1).MaximumLength(100);
    }
}
