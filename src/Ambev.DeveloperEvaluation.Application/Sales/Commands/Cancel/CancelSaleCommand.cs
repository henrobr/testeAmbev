using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.Cancel;

/// <summary>
/// Represents a command to cancel a sale.
/// </summary>
/// <param name="SaleId">The unique identifier of the sale to be cancelled.</param>
public sealed record CancelSaleCommand(int SaleId) : IRequest<bool>;
