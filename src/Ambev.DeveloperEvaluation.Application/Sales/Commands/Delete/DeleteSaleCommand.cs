using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.Delete;

/// <summary>
/// Command to delete an existing sale.
/// </summary>
public sealed record DeleteSaleCommand(int SaleId) : IRequest<bool> { }