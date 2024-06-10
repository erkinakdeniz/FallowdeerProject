using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.MainFeatures.Library.Queries.Files;
public class GetLibraryFileForPopupQueryValidator : AbstractValidator<GetLibraryFileForPopupQuery>
{
    public GetLibraryFileForPopupQueryValidator()
    {
        RuleFor(x => x.Directory).Must(x => x != null && x.StartsWith("Library/")).WithMessage("Directory 'Library/' ile başlamalıdır. Geçersiz dizin belirttiniz!");
    }
}
