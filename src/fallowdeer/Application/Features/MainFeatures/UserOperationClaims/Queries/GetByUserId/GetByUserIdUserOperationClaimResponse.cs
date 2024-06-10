using Core.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.MainFeatures.UserOperationClaims.Queries.GetByUserId;
public class GetByUserIdUserOperationClaimResponse : IResponse
{
    public int Id { get; set; }
    public int OperationClaimId { get; set; }
    public string Name { get; set; }
}
