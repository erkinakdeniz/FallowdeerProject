using Core.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.MainFeatures.OperationClaims.Queries.GetAll;
public class OperationClaimResponse : IResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
}
