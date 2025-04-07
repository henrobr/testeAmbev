using Ambev.DeveloperEvaluation.Application.Sales.Commands.Cancel;
using Ambev.DeveloperEvaluation.Domain.Models.SaleAggregate.Entities;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData;

/// <summary>
/// Provides test data for <see cref="CancelSaleCommand"/> related tests.
/// </summary>
public static class CancelSaleCommandTestData
{
    /// <summary>
    /// Creates a valid command with a given sale ID.
    /// </summary>
    /// <param name="saleId">The ID of the sale.</param>
    /// <returns>A valid <see cref="CancelSaleCommand"/>.</returns>
    public static CancelSaleCommand CreateValidCommand(int saleId)
    {
        return new CancelSaleCommand(saleId);
    }

    /// <summary>
    /// Creates a sale with status 'Pending'.
    /// </summary>
    /// <param name="customerId">The ID of the customer.</param>
    /// <param name="branchId">The ID of the branch.</param>
    /// <returns>A pending <see cref="Sale"/>.</returns>
    public static Sale CreatePendingSale(Guid customerId, Guid branchId)
    {
        return Sale.Create(customerId, branchId);
    }

    /// <summary>
    /// Creates a sale with status 'Cancelled'.
    /// </summary>
    /// <param name="customerId">The ID of the customer.</param>
    /// <param name="branchId">The ID of the branch.</param>
    /// <returns>A cancelled <see cref="Sale"/>.</returns>
    public static Sale CreateCancelledSale(Guid customerId, Guid branchId)
    {
        var sale = Sale.Create(customerId, branchId);
        sale.Cancel();
        return sale;
    }
}
