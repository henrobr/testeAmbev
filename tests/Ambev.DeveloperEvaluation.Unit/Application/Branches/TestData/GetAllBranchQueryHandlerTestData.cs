using Ambev.DeveloperEvaluation.Domain.Models.BranchAggregate.DTOs;

namespace Ambev.DeveloperEvaluation.Unit.Application.Branches.TestData;

/// <summary>
/// Provides test data for <see cref="GetAllBranchQueryHandlerTests"/>.
/// </summary>
public static class GetAllBranchQueryHandlerTestData
{
    /// <summary>
    /// Generates a full list of branch DTOs for testing purposes.
    /// </summary>
    /// <returns>An <see cref="IQueryable{BranchDto}"/> representing all branches.</returns>
    public static IQueryable<BranchDto> GenerateAllBranches()
    {
        return new List<BranchDto>
        {
            new(Guid.NewGuid(), "Branch - SP"),
            new(Guid.NewGuid(), "Branch - RJ"),
            new(Guid.NewGuid(), "Branch - MG")
        }.AsQueryable();
    }

    /// <summary>
    /// Filters the full branch list by the given name.
    /// </summary>
    /// <param name="name">The name filter to apply.</param>
    /// <returns>A filtered <see cref="IQueryable{BranchDto}"/>.</returns>
    public static IQueryable<BranchDto> GenerateFilteredBranches(string name)
    {
        return GenerateAllBranches()
            .Where(b => b.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
            .AsQueryable();
    }
}