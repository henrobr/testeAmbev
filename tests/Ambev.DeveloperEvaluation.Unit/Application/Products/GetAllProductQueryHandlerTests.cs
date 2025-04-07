using Ambev.DeveloperEvaluation.Application.Products.Queries.GetAll;
using Ambev.DeveloperEvaluation.Domain.Models.ProductAggregate.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.Products.TestData;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products;

/// <summary>
/// Unit tests for the <see cref="GetAllProductQueryHandler"/>.
/// </summary>
public class GetAllProductQueryHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly GetAllProductQueryHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAllProductQueryHandlerTests"/> class.
    /// </summary>
    public GetAllProductQueryHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _handler = new GetAllProductQueryHandler(_productRepository);
    }

    /// <summary>
    /// Tests that all products are returned when no filter is provided.
    /// </summary>
    [Fact(DisplayName = "Given no filter When querying products Then returns all products")]
    public async Task Handle_WithoutFilter_ReturnsAllProducts()
    {
        // Arrange
        var expectedProducts = GetAllProductQueryHandlerTestData.GenerateAllProducts();
        _productRepository.GetAllAsNoTracking(null).Returns(expectedProducts);
        var query = new GetAllProductQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedProducts);
    }

    /// <summary>
    /// Tests that products matching the filter name are returned.
    /// </summary>
    [Fact(DisplayName = "Given name filter When querying products Then returns filtered products")]
    public async Task Handle_WithFilter_ReturnsFilteredProducts()
    {
        // Arrange
        var filter = "TESTE 1";
        var expectedFiltered = GetAllProductQueryHandlerTestData.GenerateFilteredProducts(filter);
        _productRepository.GetAllAsNoTracking(filter).Returns(expectedFiltered);
        var query = new GetAllProductQuery(filter);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedFiltered);
    }
}
