namespace Core.CrossCuttingConcerns.Exceptions.Types;
public class OperationFailedException: Exception
{
    public IEnumerable<OperationFailedExceptionModel> Errors { get; set; }
    public string Detail { get; set; }
    public OperationFailedException():base()
    {
        Errors=Array.Empty<OperationFailedExceptionModel>();
    }
    public OperationFailedException(string? message):base(message)
    {
        Errors = Array.Empty<OperationFailedExceptionModel>();
    }
    public OperationFailedException(IEnumerable<OperationFailedExceptionModel> errors):base(BuildErrorMessage(errors))
    {
        Errors = errors;
    }
    public OperationFailedException(string detail,IEnumerable<OperationFailedExceptionModel> errors) : base(BuildErrorMessage(errors))
    {
        Errors = errors;
        Detail = detail;
    }
    public static string BuildErrorMessage(IEnumerable<OperationFailedExceptionModel> errors) {
        IEnumerable<string> arr = errors.Select(x => $"{Environment.NewLine} -- {x.Id}: Name:{x.Name}, Description:{x.Description}");
        return $"Operation failed: {string.Join(string.Empty, arr)}";
    }
}
public class OperationFailedExceptionModel
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}
