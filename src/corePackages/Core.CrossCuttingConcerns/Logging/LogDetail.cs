namespace Core.CrossCuttingConcerns.Logging;

public class LogDetail
{
    public string FullName { get; set; }
    public string MethodName { get; set; }
    public string User { get; set; }
    public Guid UserId { get; set; }
    public string Email { get; set; }
    public string IPAddress { get; set; }
    public List<LogParameter> Parameters { get; set; }

    public LogDetail()
    {
        FullName = string.Empty;
        MethodName = string.Empty;
        User = string.Empty;
        Email = string.Empty;
        UserId = Guid.Empty;
        IPAddress = string.Empty;
        Parameters = new List<LogParameter>();
    }

    public LogDetail(string fullName, string methodName, string user, Guid userId,string mail,string ipAddress, List<LogParameter> parameters)
    {
        FullName = fullName;
        MethodName = methodName;
        User = user;
        Parameters = parameters;
        UserId=userId;
        Email = mail;
        IPAddress = ipAddress;
    }
}
