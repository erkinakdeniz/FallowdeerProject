using Core.CrossCuttingConcerns.Exceptions.Handlers;
using Core.CrossCuttingConcerns.Extensions;
using Core.CrossCuttingConcerns.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog;

using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Core.CrossCuttingConcerns.Exceptions;

public class ExceptionMiddleware
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly HttpExceptionHandler _httpExceptionHandler;
    private readonly LoggerServiceBase _loggerService;
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next, IHttpContextAccessor contextAccessor, LoggerServiceBase loggerService)
    {
        _next = next;
        _contextAccessor = contextAccessor;
        _loggerService = loggerService;
        _httpExceptionHandler = new HttpExceptionHandler();
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            await LogException(context, exception);
            await HandleExceptionAsync(context.Response, exception);
        }
    }

    private Task HandleExceptionAsync(HttpResponse response, Exception exception)
    {
        response.ContentType = "application/json";
        _httpExceptionHandler.Response = response;
        return _httpExceptionHandler.HandleExceptionAsync(exception);
    }

    private Task LogException(HttpContext context, Exception exception)
    {
        List<LogParameter> logParameters =
            new()
            {
                new LogParameter { Type = context.GetType().Name, Value = exception.Message.ToString() }
            };
        
        LogDetail logDetail =
            new()
            {
                MethodName = _next.Method.Name,
                Parameters = logParameters,
                //User = _contextAccessor.HttpContext?.User.Identity?.Name ?? "?"
                User = _contextAccessor.HttpContext.Items[MyClaimTypes.Name]?.ToString() ?? "?",
                Email= _contextAccessor.HttpContext.Items[MyClaimTypes.Email]?.ToString() ?? "?",
                UserId= Guid.Parse(_contextAccessor.HttpContext?.Items[MyClaimTypes.ID]?.ToString() ?? "00000000-0000-0000-0000-000000000000"),
                IPAddress=_contextAccessor.HttpContext.Connection.RemoteIpAddress.ToString() ?? "000.000.000.000",
            };

        _loggerService.Info(JsonSerializer.Serialize(logDetail));
        return Task.CompletedTask;
    }
}
