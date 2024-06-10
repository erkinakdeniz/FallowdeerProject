using FluentValidation;

namespace Application.Features.MainFeatures.Library.Commands.Delete;
public class DeleteLibraryFolderCommandValidator : AbstractValidator<DeleteLibraryFolderCommand>
{
    public DeleteLibraryFolderCommandValidator()
    {
        RuleFor(x => x.Directory).Must(x => x != null && x.StartsWith("Library/")).WithMessage("Directory 'Library/' ile başlamalıdır. Geçersiz dizin belirttiniz!");
    }
}
