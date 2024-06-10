using Application.Services.Repositories;
using Core.Persistence.Repositories;
using Core.Security.Entities;
using Persistence.Contexts;

namespace Persistence.Repositories.MainRepositories;
public class UnicodeRepository : EfRepositoryBase<Unicode, int, BaseDbContext>, IUnicodeRepository
{
    public UnicodeRepository(BaseDbContext context) : base(context)
    {
    }
}
