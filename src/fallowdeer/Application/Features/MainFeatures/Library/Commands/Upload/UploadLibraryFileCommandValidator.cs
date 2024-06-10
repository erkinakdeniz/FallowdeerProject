using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Application.Features.MainFeatures.Library.Commands.Upload;
public class UploadLibraryFileCommandValidator : AbstractValidator<UploadLibraryFileCommand>
{
    public UploadLibraryFileCommandValidator()
    {
        RuleFor(x => x.FormFiles).NotNull().NotEmpty().Must((data, files) => HasValidExtension(files, data.Directory)).WithMessage("Geçersiz dosya uzantısı.");
        RuleFor(x => x.Directory).Must(x => x != null && x.StartsWith("Library/")).WithMessage("Directory 'Library/' ile başlamalıdır. Geçersiz dizin belirttiniz!");
    }
    private bool HasValidExtension(IFormFile[] files, string directory)
    {
        var validDocExtensions = new[] { ".xls", ".xlsx", ".doc", ".docx", ".pdf" };
        var validImageExtensions = new[] { ".jpg", ".jpeg", ".png", ".svg", ".webp" };
        foreach (var file in files)
        {
            var extension = Path.GetExtension(file.FileName);
            if (directory.Contains("Library/Dosyalar"))
            {
                bool valid = validDocExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase);
                if (!valid)
                    return false;
            }
            else if (directory.Contains("Library/Resimler"))
            {
                bool valid = validImageExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase);
                if (!valid)
                    return false;
            }
            else
                return false;


        }
        return true;

    }
}
