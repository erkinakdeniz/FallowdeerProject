using Core.Persistence.Repositories;
using Core.Security.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Repositories;
public interface IUnicodeRepository: IAsyncRepository<Unicode, int>, IRepository<Unicode, int>
{
}
