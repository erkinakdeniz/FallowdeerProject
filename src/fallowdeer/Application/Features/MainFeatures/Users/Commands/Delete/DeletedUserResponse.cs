using Core.Application.Responses;

namespace Application.Features.MainFeatures.Users.Commands.Delete;

public class DeletedUserResponse : IResponse
{
    public Guid Id { get; set; }
}
