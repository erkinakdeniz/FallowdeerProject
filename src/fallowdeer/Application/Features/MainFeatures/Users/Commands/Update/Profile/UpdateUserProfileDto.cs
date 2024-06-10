using Core.Application.Dtos;
using Core.Application.Pipelines.Authorization;

namespace Application.Features.MainFeatures.Users.Commands.Update.Profile;
public class UpdateUserProfileDto : ISecured,IDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string CurrentPassword { get; set; }
}
