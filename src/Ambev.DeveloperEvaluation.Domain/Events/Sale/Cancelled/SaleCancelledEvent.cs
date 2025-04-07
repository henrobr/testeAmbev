using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events.Sale.Cancelled;

/// <summary>
/// Event triggered when a sale is cancelled.
/// </summary>
public sealed record SaleCancelledEvent(int SaleId) : INotification;