using Ambev.DeveloperEvaluation.Domain.Models.BranchAggregate.DTOs;
using Ambev.DeveloperEvaluation.Domain.Models.BranchAggregate.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.Queries.GetAll;

/// <summary>
/// Handles the <see cref="GetAllBranchQuery"/> to retrieve all branches, optionally filtered by name.
/// </summary>
/// <param name="branchRepository">The repository for accessing branch data.</param>
public sealed class GetAllBranchQueryHandler(IBranchRepository branchRepository)
    : IRequestHandler<GetAllBranchQuery, IQueryable<BranchDto>>
{
    /// <summary>
    /// Handles the request to get all branches.
    /// </summary>
    /// <param name="request">The query request containing the optional name filter.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A queryable list of branches matching the filter criteria.</returns>
    public async Task<IQueryable<BranchDto>> Handle(GetAllBranchQuery request, 
        CancellationToken cancellationToken)
    {
        return branchRepository.GetAllAsNoTracking(request.Name);
    }
}