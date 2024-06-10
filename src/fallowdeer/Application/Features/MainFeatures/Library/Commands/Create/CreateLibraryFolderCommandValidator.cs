using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.MainFeatures.Library.Commands.Create;
public class CreateLibraryFolderCommandValidator : AbstractValidator<CreateLibraryFolderCommand>
{
    public CreateLibraryFolderCommandValidator()
    {
        RuleFor(x => x.Directory).Must(x => x != null && x.StartsWith("Library/")).WithMessage("Directory 'Library/' ile başlamalıdır. Geçersiz dizin belirttiniz!");
    }
}
