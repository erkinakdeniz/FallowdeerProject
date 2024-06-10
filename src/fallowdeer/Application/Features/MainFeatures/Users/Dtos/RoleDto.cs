using Core.Application.Dtos;

namespace Application.Features.MainFeatures.Users.Dtos;
public class RoleDto:IDto
{
    public int Id { get; set; }
    public string Name { get; set; }
}
