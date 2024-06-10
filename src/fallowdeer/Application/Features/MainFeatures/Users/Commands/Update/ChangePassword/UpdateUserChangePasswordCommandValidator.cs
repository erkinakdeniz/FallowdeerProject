using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.MainFeatures.Users.Commands.Update.ChangePassword;
public class UpdateUserChangePasswordCommandValidator : AbstractValidator<UpdateUserChangePasswordCommand>
{
    public UpdateUserChangePasswordCommandValidator()
    {
        RuleFor(x => x.Password).MinimumLength(6).MaximumLength(16);
    }
}
