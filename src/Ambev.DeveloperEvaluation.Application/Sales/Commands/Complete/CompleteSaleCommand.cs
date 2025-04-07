using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.Complete;

/// <summary>
/// Represents a command to cancel a sale.
/// </summary>
/// <param name="SaleId">The unique identifier of the sale to be cancelled.</param>
public sealed record CompleteSaleCommand(int SaleId) : IRequest<bool>;
