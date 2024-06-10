using Core.Application.Pipelines.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.MainFeatures.Auth.Commands.VerifyOtpAuthenticator;
public class VerifyOtpAuthenticatorDto : ISecured
{
    public string ActivationKey { get; set; }
}
