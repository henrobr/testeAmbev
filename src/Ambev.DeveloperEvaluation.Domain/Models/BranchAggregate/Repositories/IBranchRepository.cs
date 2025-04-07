using Ambev.DeveloperEvaluation.Domain.Models.BranchAggregate.DTOs;
using Ambev.DeveloperEvaluation.Domain.Models.BranchAggregate.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Models.BranchAggregate.Repositories;

public interface IBranchRepository
{
    Task CreateAsync(Branch branch, CancellationToken cancellationToken);
    Task<Branch> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Branch> GetByIdAsNoTrackingAsync(Guid id,
        CancellationToken cancellationToken);
    IQueryable<BranchDto> GetAllAsNoTracking(string? name = null);

}
