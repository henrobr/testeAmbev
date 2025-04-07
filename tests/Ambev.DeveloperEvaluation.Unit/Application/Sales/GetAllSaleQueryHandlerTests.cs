using Ambev.DeveloperEvaluation.Application.Sales.Queries.GetListAll;
using Ambev.DeveloperEvaluation.Domain.Models.SaleAggregate.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

/// <summary>
/// Unit tests for <see cref="GetAllSaleQueryHandler"/>.
/// </summary>
public sealed class GetAllSaleQueryHandlerTests
{
    private readonly ISaleRepository _saleRepository = Substitute.For<ISaleRepository>();
    private readonly GetAllSaleQueryHandler _handler;

    public GetAllSaleQueryHandlerTests()
    {
        _handler = new GetAllSaleQueryHandler(_saleRepository);
    }

    /// <summary>
    /// Should return all sales when no filters are provided.
    /// </summary>
    [Fact]
    public async Task Handle_NoFilters_ReturnsAllSales()
    {
        // Arrange
        var query = new GetAllSaleQuery();
        var expected = GetAllSaleQueryTestData.CreateSales();
        _saleRepository.GetAllAsNoTracking(null, null, null).Returns(expected);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(expected.Count());
    }

    /// <summary>
    /// Should filter sales by sale ID.
    /// </summary>
    [Fact]
    public async Task Handle_FilterBySaleId_ReturnsCorrectSale()
    {
        // Arrange
        var expected = GetAllSaleQueryTestData.CreateSales().ToList();
        var targetSale = expected.First();

        _saleRepository.GetAllAsNoTracking(targetSale.Id, null, null)
            .Returns(expected.Where(x => x.Id == targetSale.Id).AsQueryable());

        var query = new GetAllSaleQuery(SaleId: targetSale.Id);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().ContainSingle(x => x.Id == targetSale.Id);
    }

    /// <summary>
    /// Should filter sales by customer or branch name.
    /// </summary>
    [Fact]
    public async Task Handle_FilterByNames_ReturnsMatchingSales()
    {
        // Arrange
        var allSales = GetAllSaleQueryTestData.CreateSales().ToList();
        var query = new GetAllSaleQuery(CustomerName: "TEST 1", BranchName: "SP");

        var filtered = allSales.Where(s =>
            s.CustomerName.Contains("TEST 1") && s.BranchName.Contains("SP")).AsQueryable();

        _saleRepository.GetAllAsNoTracking(null, "TEST 1", "SP").Returns(filtered);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(filtered);
    }
}
