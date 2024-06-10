using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Core.Persistence.Repositories;

public interface IQuery<T>
{
    IQueryable<T> Query();
    IQueryable<T> Query(Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, bool withDeleted = false,
        bool enableTracking = true);
    Task<List<T>> QueryAsync(Expression<Func<T, bool>>? predicate = null,
      Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
      Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, bool withDeleted = false,
      bool enableTracking = true);
}
