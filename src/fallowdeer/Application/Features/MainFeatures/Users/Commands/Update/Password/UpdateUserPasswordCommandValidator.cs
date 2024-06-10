using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.MainFeatures.Users.Commands.Update.Password;
public class UpdateUserPasswordCommandValidator : AbstractValidator<UpdateUserPasswordCommand>
{
    public UpdateUserPasswordCommandValidator()
    {
        RuleFor(x => x.PasswordDto.NewPassword).MinimumLength(6).MaximumLength(16);
        RuleFor(x => x.PasswordDto.CurrentPassword).NotNull().NotEmpty();
    }
}
