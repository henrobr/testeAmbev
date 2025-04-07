using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events.Sale.Completed;

/// <summary>
/// Event triggered when a sale is completed.
/// </summary>
public sealed record SaleCompletedEvent(int SaleId)
    : INotification;
