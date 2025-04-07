using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events.Sale.Modified;

/// <summary>
/// Event triggered when a sale is modified.
/// </summary>
public sealed record SaleModifiedEvent(
    int SaleId, 
    Guid CustomerId, 
    Guid BranchId,
    decimal TotalAmount)
    : INotification;
