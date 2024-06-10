namespace Core.CrossCuttingConcerns.Logging;

public class LogDetailWithException : LogDetail
{
    public string ExceptionMessage { get; set; }

    public LogDetailWithException()
    {
        ExceptionMessage = string.Empty;
    }

    public LogDetailWithException(string fullName, string methodName, string user, Guid userId,string mail,string ipAddress, List<LogParameter> parameters, string exceptionMessage)
        : base(fullName, methodName, user,userId,mail,ipAddress, parameters)
    {
        ExceptionMessage = exceptionMessage;
    }
}
