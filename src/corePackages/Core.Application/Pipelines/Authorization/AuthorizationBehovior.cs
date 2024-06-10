using Core.CrossCuttingConcerns;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Core.CrossCuttingConcerns.Extensions;
using Core.Security.JWT;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Core.Application.Pipelines.Authorization;
public class AuthorizationBehovior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ISecured
{
    private readonly ITokenHelper _token;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthorizationBehovior(ITokenHelper token, IHttpContextAccessor httpContextAccessor)
    {
        _token = token ?? throw new ArgumentNullException(nameof(token));
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        
        string authorization = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
        if (string.IsNullOrWhiteSpace(authorization))
            throw new AuthorizationException(SecurityMessages.AuthorizationException);
        string token = "";
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
            

        if (request is ISecuredAndUserId userIdentity)
        {
            userIdentity.UserId = Guid.Parse(userId.ToString() ?? Guid.Empty.ToString());
        }
        TResponse response = await next();
        return response;
    }

}

