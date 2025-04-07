using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.Update;

/// <summary>
/// Command to update an existing sale.
/// </summary>
public sealed record UpdateSaleCommand(
    int SaleId,
    Guid CustomerId,
    Guid BranchId,
    List<SaleItems> Items)
    : IRequest<bool>;
    public sealed record SaleItems(int ProductId, int Quantity);