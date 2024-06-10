using Core.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.MainFeatures.Auth.Commands.Login.OTPLogin;
public class OtpLoginCommandDto : IDto
{
    public string AuthenticatorCode { get; set; }
    public string Email { get; set; }
}
