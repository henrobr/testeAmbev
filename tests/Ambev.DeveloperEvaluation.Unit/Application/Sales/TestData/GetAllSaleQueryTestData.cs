using Ambev.DeveloperEvaluation.Domain.Models.SaleAggregate.DTOs;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData;

/// <summary>
/// Provides test data for <see cref="GetAllSaleQueryHandlerTests"/>.
/// </summary>
public static class GetAllSaleQueryTestData
{
    /// <summary>
    /// Creates a sample list of sales for testing purposes.
    /// </summary>
    public static IQueryable<SaleDto> CreateSales()
    {
        return new List<SaleDto>
        {
            new(
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
                }),

            new(
                Id: 2,
                StatusId: 2,
                StatusName: "Cancelled",
                CustomerId: Guid.NewGuid(),
                CustomerName: "CLIENT TEST 2",
                BranchId: Guid.NewGuid(),
                BranchName: "BRANCH - RJ",
                Items: new List<SaleItemDto>
                {
                    new(3, 2, 3, "PRODUCT TEST - 3", 1, 30, 5)
                })
        }.AsQueryable();
    }
}
