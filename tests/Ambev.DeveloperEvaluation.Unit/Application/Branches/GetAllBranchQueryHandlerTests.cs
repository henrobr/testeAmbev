using Ambev.DeveloperEvaluation.Application.Branches.Queries.GetAll;
using Ambev.DeveloperEvaluation.Domain.Models.BranchAggregate.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.Branches.TestData;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Branches;

/// <summary>
/// Unit tests for the <see cref="GetAllBranchQueryHandler"/>.
/// </summary>
public class GetAllBranchQueryHandlerTests
{
    private readonly IBranchRepository _branchRepository;
    private readonly GetAllBranchQueryHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAllBranchQueryHandlerTests"/> class.
    /// </summary>
    public GetAllBranchQueryHandlerTests()
    {
        _branchRepository = Substitute.For<IBranchRepository>();
        _handler = new GetAllBranchQueryHandler(_branchRepository);
    }

    /// <summary>
    /// Tests that when no filter is provided, all branches are returned.
    /// </summary>
    [Fact(DisplayName = "Given no filter When querying branches Then returns all branches")]
    public async Task Handle_WithoutFilter_ReturnsAllBranches()
    {
        // Arrange
        var expectedBranches = GetAllBranchQueryHandlerTestData.GenerateAllBranches();
        _branchRepository.GetAllAsNoTracking(null).Returns(expectedBranches);
        var query = new GetAllBranchQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedBranches);
    }

    /// <summary>
    /// Tests that when a name filter is provided, only matching branches are returned.
    /// </summary>
    [Fact(DisplayName = "Given name filter When querying branches Then returns filtered branches")]
    public async Task Handle_WithFilter_ReturnsFilteredBranches()
    {
        // Arrange
        var filter = "SP";
        var expectedFiltered = GetAllBranchQueryHandlerTestData.GenerateFilteredBranches(filter);
        _branchRepository.GetAllAsNoTracking(filter).Returns(expectedFiltered);
        var query = new GetAllBranchQuery(filter);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedFiltered);
    }
}