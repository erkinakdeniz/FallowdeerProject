using Application.Services.Repositories;
using Core.Persistence.Repositories;
using Core.Security.Entities;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.MainRepositories;
public class SCategoryRepository : EfRepositoryBase<SCategory, int, BaseDbContext>, ISCategoryRepository
{
    public SCategoryRepository(BaseDbContext context) : base(context)
    {
    }
}
