using Core.CrossCuttingConcerns;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Core.CrossCuttingConcerns.Extensions;
using Core.Security.Constants;
using Core.Security.JWT;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace Core.Application.Pipelines.Authorization;

public class RoleBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest:IRoleRequest
    
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITokenHelper _token;

    public RoleBehavior(IHttpContextAccessor httpContextAccessor, ITokenHelper token)
    {
        _httpContextAccessor = httpContextAccessor;
        _token = token;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {

        string? authorization = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
        string token = "";
        if (authorization.IsNullOrEmpty())
            throw new AuthorizationException(SecurityMessages.AuthorizationException);
        string[] jwtToken = authorization.Split(" ");
        if (jwtToken.Length > 1)
        {
            token = jwtToken[1];
        }
        else
        {
            token = authorization;
        }
        var jwt = _token.TokenVerify(token);
        if (jwt.Payload.TryGetValue(MyClaimTypes.ID, out var userId))
        {
            _httpContextAccessor.HttpContext.Items[MyClaimTypes.ID] = userId;
        }
        else
        {
            throw new AuthorizationException(SecurityMessages.AuthorizationException);
        }

        if (jwt.Payload.TryGetValue(MyClaimTypes.Name, out var name))
        {
            _httpContextAccessor.HttpContext.Items[MyClaimTypes.Name] = name;
        }
        else
        {
            throw new AuthorizationException(SecurityMessages.AuthorizationException);
        }

        if (jwt.Payload.TryGetValue(MyClaimTypes.Email, out var email))
        {
            _httpContextAccessor.HttpContext.Items[MyClaimTypes.Email] = email;
        }
        else
        {
            throw new AuthorizationException(SecurityMessages.AuthorizationException);
        }
        var roles = jwt.Payload.Where(x => x.Key.Equals(MyClaimTypes.Role)).Select(x => x.Value.ToString()).ToList();
        if (request is IRoleAndUserIdRequest userIdRequest)
        {
            userIdRequest.UserId = Guid.Parse(userId.ToString() ?? Guid.Empty.ToString());
        }

        bool isNotMatchedAUserRoleClaimWithRequestRoles = roles
            .FirstOrDefault(
                userRoleClaim => userRoleClaim == GeneralOperationClaims.Admin || request.Roles.Any(role => role == userRoleClaim)
            )
            .IsNullOrEmpty();
        if (isNotMatchedAUserRoleClaimWithRequestRoles)
            throw new AuthorizationException(SecurityMessages.AuthorizationException);

        TResponse response = await next();
        return response;

    }
}
