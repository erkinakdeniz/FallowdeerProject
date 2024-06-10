using Core.CrossCuttingConcerns.Exceptions.Types;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.Exceptions.HttpProblemDetails;
internal class OperationFailedProblemDetails:ProblemDetails
{
    public IEnumerable<OperationFailedExceptionModel> Errors { get; set; }
    public OperationFailedProblemDetails(string? detail,IEnumerable<OperationFailedExceptionModel> errors)
    {
        Title = "Operation Failed";
        Status = 434;
        Detail = detail;
        Errors = errors;
    }
}
