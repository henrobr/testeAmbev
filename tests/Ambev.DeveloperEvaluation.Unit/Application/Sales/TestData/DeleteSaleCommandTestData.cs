using Ambev.DeveloperEvaluation.Domain.Models.SaleAggregate.Entities;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData;

/// <summary>
/// Provides test data for <see cref="DeleteSaleCommandHandlerTests"/>.
/// </summary>
public static class DeleteSaleCommandTestData
{
    /// <summary>
    /// Creates a valid sale with a confirmed status.
    /// </summary>
    public static Sale CreateValidSale()
    {
        var sale = Sale.Create(Guid.NewGuid(), Guid.NewGuid());

        typeof(Sale)
            .GetProperty(nameof(Sale.Id))!
            .SetValue(sale, 42);

        return sale;
    }
}
