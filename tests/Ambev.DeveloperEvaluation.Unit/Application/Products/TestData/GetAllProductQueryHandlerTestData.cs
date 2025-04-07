using Ambev.DeveloperEvaluation.Domain.Models.ProductAggregate.DTOs;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products.TestData;

/// <summary>
/// Provides test data for <see cref="GetAllProductQueryHandlerTests"/>.
/// </summary>
public static class GetAllProductQueryHandlerTestData
{
    /// <summary>
    /// Generates a full list of product DTOs for testing.
    /// </summary>
    /// <returns>An <see cref="IQueryable{ProductDto}"/> representing all products.</returns>
    public static IQueryable<ProductDto> GenerateAllProducts()
    {
        return new List<ProductDto>
        {
            new(1, "PRODUCT TEST 1", 3.80m),
            new(2, "PRODUCT TEST 2", 4.30m),
            new(3, "PRODUCT TEST 3", 4.99m)
        }.AsQueryable();
    }

    /// <summary>
    /// Filters the product list by name.
    /// </summary>
    /// <param name="name">The name to filter by.</param>
    /// <returns>A filtered <see cref="IQueryable{ProductDto}"/>.</returns>
    public static IQueryable<ProductDto> GenerateFilteredProducts(string name)
    {
        return GenerateAllProducts()
            .Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
            .AsQueryable();
    }
}
