using Core.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.MainFeatures.Auth.Commands.Login.EmailLogin;
public class EmailLoginDto : IDto
{
    public string Email { get; set; }
    public string Code { get; set; }
}
