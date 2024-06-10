using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.MainFeatures.Auth.Commands.ForgotPassword;
public class ForgotPasswordUserControlCommandValidator : AbstractValidator<ForgotPasswordUserControlCommand>
{
    public ForgotPasswordUserControlCommandValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
    }
}
