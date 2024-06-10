using Application.Features.MainFeatures.Auth.Constants;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.MainFeatures.Auth.Commands.ResetPassword;
public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.Token).NotNull().NotEmpty();
        RuleFor(x => x.Password).NotEmpty().NotNull().Equal(x => x.PasswordConfirm).WithMessage(AuthMessages.PasswordConfirm);
    }
}
