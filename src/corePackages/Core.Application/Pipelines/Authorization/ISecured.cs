using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Pipelines.Authorization;
public interface ISecured
{
}
public interface ISecuredAndUserId:ISecured
{
    public Guid UserId { get; set; }
}
