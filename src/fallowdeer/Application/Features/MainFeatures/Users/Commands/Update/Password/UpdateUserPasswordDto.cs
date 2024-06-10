using Core.Application.Dtos;
using Core.Application.Pipelines.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.MainFeatures.Users.Commands.Update.Password;
public class UpdateUserPasswordDto : ISecured,IDto
{
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
}
