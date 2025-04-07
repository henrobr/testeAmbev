using System.Linq.Expressions;

namespace Ambev.DeveloperEvaluation.Domain.Common;

public interface IUnitOfWork
{
    Task<bool> CommitAsync(CancellationToken cancellationToken);

    IQueryable<T> QueryAsNoTracking<T>
        (params Expression<Func<T, object>>[] includeProperties) where T : class;
}
