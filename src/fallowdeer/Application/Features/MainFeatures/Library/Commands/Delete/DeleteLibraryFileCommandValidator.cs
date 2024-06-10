using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.MainFeatures.Library.Commands.Delete;
public class DeleteLibraryFileCommandValidator : AbstractValidator<DeleteLibraryFileCommand>
{
    public DeleteLibraryFileCommandValidator()
    {
        RuleFor(x => x.ImageSrc).Must(x => x != null && x.StartsWith("Library/")).WithMessage("ImageSrc 'Library/' ile başlamalıdır. Geçersiz dizin belirttiniz!");
    }
}
