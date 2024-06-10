using Core.CrossCuttingConcerns.Exceptions.Types;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.MainControllers;
public class BaseController : ControllerBase
{
    protected IMediator Mediator =>
        _mediator ??=
            HttpContext.RequestServices.GetService<IMediator>()
            ?? throw new InvalidOperationException("IMediator cannot be retrieved from request services.");

    private IMediator? _mediator;

    protected string getIpAddress()
    {
        string ipAddress = Request.Headers.ContainsKey("X-Forwarded-For")
            ? Request.Headers["X-Forwarded-For"].ToString()
            : HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString()
                ?? throw new InvalidOperationException("IP address cannot be retrieved from request.");
        return ipAddress;
    }
    //protected object getUserIdFromCookies() => HttpContext.Request.Cookies[MyClaimTypes.ID] ?? throw new AuthorizationException(SecurityMessages.AuthenticatedException);

    protected string getRefreshTokenFromHeader() =>
         Request.Headers["Referer-Ux"].ToString() ?? throw new BusinessException("token okumadı");
    //HttpContext.Items["Referer-Ux"] as string;

    protected void setRefreshTokenToHeader(string refreshToken)
    {
        Response.Headers["Referer-Ux"] = refreshToken;
        //HttpContext.Items["Referer-Ux"]= refreshToken;
    }

    //protected void setRefreshTokenToCookie(RefreshToken refreshToken)
    //{
    //    CookieOptions cookieOptions = new() { HttpOnly = true, Expires = DateTime.UtcNow.AddDays(7), Secure = true };
    //    Response.Cookies.Append(key: "kkrt", refreshToken.Token, cookieOptions);

    //}
}
