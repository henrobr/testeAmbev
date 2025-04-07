using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events.Sale.Created;

/// <summary>
/// Event triggered when a sale is created.
/// </summary>
public sealed record SaleCreatedEvent(
    int SaleId, 
    Guid CustomerId,
    Guid BranchId,
    decimal TotalAmount)
    : INotification;
