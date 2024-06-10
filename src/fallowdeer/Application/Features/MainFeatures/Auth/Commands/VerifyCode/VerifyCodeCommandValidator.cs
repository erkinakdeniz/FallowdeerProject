using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.MainFeatures.Auth.Commands.VerifyCode;
public class VerifyCodeCommandValidator : AbstractValidator<VerifyCodeCommand>
{
    public VerifyCodeCommandValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Code).NotNull().NotEmpty().MaximumLength(10).Matches(@"^[A-Za-z\s0-9ğüşıöçİĞÜŞÖÇ]*$").WithMessage("'{PropertyName}' sadece harfler, boşluklar ve rakamlar içermelidir.");
    }
}
