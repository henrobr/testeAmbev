using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events.Sale.Deleted;

/// <summary>
/// Event triggered when a sale is cancelled.
/// </summary>
public sealed record SaleDeletedEvent(int SaleId) : INotification;