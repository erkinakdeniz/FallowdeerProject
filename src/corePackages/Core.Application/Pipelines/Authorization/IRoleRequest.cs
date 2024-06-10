namespace Core.Application.Pipelines.Authorization;

public interface IRoleRequest
{
    public string[] Roles { get; }
}
public interface IRoleAndUserIdRequest:IRoleRequest
{
    public Guid UserId { get; set; }
}
