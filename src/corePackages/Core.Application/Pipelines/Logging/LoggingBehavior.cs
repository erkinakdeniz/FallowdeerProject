using Core.CrossCuttingConcerns.Extensions;
using Core.CrossCuttingConcerns.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Core.Application.Pipelines.Logging;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, ILoggableRequest
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly LoggerServiceBase _loggerServiceBase;

    public LoggingBehavior(LoggerServiceBase loggerServiceBase, IHttpContextAccessor httpContextAccessor)
    {
        _loggerServiceBase = loggerServiceBase;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        List<LogParameter> logParameters =
            new()
            {
                new LogParameter { Type = request.GetType().Name, Value = request }
            };

        LogDetail logDetail =
            new()
            {
                MethodName = next.Method.Name,
                Parameters = logParameters,
                //User = _httpContextAccessor.HttpContext.User.Identity?.Name ?? "?"
                User = _httpContextAccessor.HttpContext.Items[MyClaimTypes.Name]?.ToString() ?? "?",
                Email = _httpContextAccessor.HttpContext.Items[MyClaimTypes.Email]?.ToString() ?? "?",
                UserId = Guid.Parse(_httpContextAccessor.HttpContext?.Items[MyClaimTypes.ID]?.ToString() ?? "00000000-0000-0000-0000-000000000000"),
                IPAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString() ?? "000.000.000.000",
            };

        _loggerServiceBase.Info(JsonSerializer.Serialize(logDetail));
        return await next();
    }
}
