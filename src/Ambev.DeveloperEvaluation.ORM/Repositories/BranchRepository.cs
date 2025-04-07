using Ambev.DeveloperEvaluation.Domain.Models.BranchAggregate.DTOs;
using Ambev.DeveloperEvaluation.Domain.Models.BranchAggregate.Entities;
using Ambev.DeveloperEvaluation.Domain.Models.BranchAggregate.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Repository implementation for managing <see cref="Branch"/> entities.
/// Provides methods to create and retrieve branch records from the database.
/// </summary>
public class BranchRepository(DefaultContext context) : IBranchRepository
{
    private readonly DbSet<Branch> _branches = context.Set<Branch>();

    /// <summary>
    /// Asynchronously adds a new branch to the database.
    /// </summary>
    /// <param name="branch">The branch entity to be added.</param>
    /// <param name="cancellationToken">Token to observe while waiting for the operation to complete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task CreateAsync(Branch branch, CancellationToken cancellationToken)
    {
        await _branches.AddAsync(branch, cancellationToken);
    }

    /// <summary>
    /// Retrieves a branch by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the branch.</param>
    /// <param name="cancellationToken">Token to observe while waiting for the operation to complete.</param>
    /// <returns>
    /// The branch entity if found; otherwise, null.
    /// </returns>
    public async Task<Branch> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _branches.SingleOrDefaultAsync(f => f.Id == id, cancellationToken);
    }

    /// <summary>
    /// Retrieves a branch by its unique identifier asynchronously with no tracking.
    /// Useful for read-only operations to improve performance.
    /// </summary>
    /// <param name="id">The unique identifier of the branch.</param>
    /// <param name="cancellationToken">Token to observe while waiting for the operation to complete.</param>
    /// <returns>
    /// The branch entity if found; otherwise, null.
    /// </returns>
    public async Task<Branch> GetByIdAsNoTrackingAsync(Guid id,
        CancellationToken cancellationToken)
    {
        return await _branches.AsNoTracking()
            .SingleOrDefaultAsync(f => f.Id == id, cancellationToken);
    }

    /// <summary>
    /// Retrieves all branches as a queryable collection without tracking changes, optionally filtering by branch name.
    /// </summary>
    /// <param name="name">Optional name filter. If provided, only branches whose names contain this value will be returned.</param>
    /// <returns>
    /// An <see cref="IQueryable{BranchDto}"/> containing the list of branches without change tracking,
    /// optionally filtered by name.
    /// </returns>
    public IQueryable<BranchDto> GetAllAsNoTracking(string? name = null)
    {
        var query = from branch in _branches.AsNoTracking()
                    select new BranchDto(branch.Id, branch.Name);

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(x => x.Name.Contains(name));

        return query.AsQueryable();
    }
}
