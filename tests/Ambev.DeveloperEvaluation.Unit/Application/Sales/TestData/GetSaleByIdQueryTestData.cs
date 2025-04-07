using Ambev.DeveloperEvaluation.Domain.Models.SaleAggregate.DTOs;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData;

/// <summary>
/// Provides test data for <see cref="GetByIdQuery"/> related scenarios.
/// </summary>
public static class GetSaleByIdQueryTestData
{
    /// <summary>
    /// Creates a valid <see cref="SaleDto"/> for testing.
    /// </summary>
    /// <param name="saleId">The ID of the sale.</param>
    /// <returns>A valid <see cref="SaleDto"/> instance.</returns>
    public static SaleDto CreateValidSaleDto(int saleId)
    {
        return new SaleDto(
            Id: 1,
            StatusId: 1,
            StatusName: "Completed",
            CustomerId: Guid.NewGuid(),
            CustomerName: "CLIENT TEST 1",
            BranchId: Guid.NewGuid(),
            BranchName: "BRANCH - SP",
            Items: new List<SaleItemDto>
            {
                new(1, 1, 1, "PRODUCT TEST - 1", 2, 10, 0),
                new(2, 1, 2, "PRODUCT TEST - 2", 10, 20, 20)
            });
    }
}
