using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.Create;

/// <summary>
/// Represents a command to create a new sale, containing customer and branch information, along with the list of sale items.
/// </summary>
/// <param name="CustomerId">The unique identifier of the customer making the purchase.</param>
/// <param name="BranchId">The unique identifier of the branch where the sale is being processed.</param>
/// <param name="Items">A list of sale items, including product details, quantity.</param>
public sealed record CreateSaleCommand(
    Guid CustomerId,
    Guid BranchId,
    List<SaleItems> Items)
    : IRequest<int>;
    public sealed record SaleItems(int ProductId, int Quantity);