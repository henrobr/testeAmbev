using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Domain.Events.Sale.Cancelled;

/// <summary>
/// Event handler responsible for processing the SaleCancelled event.
/// </summary>
public sealed class SaleCancelledEventHandler(
    ILogger<SaleCancelledEventHandler> logger) 
    : INotificationHandler<SaleCancelledEvent>
{
    /// <summary>
    /// Handles the SaleCancelled event by logging the sale cancellation.
    /// </summary>
    public Task Handle(SaleCancelledEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Sale cancelled: ID {notification.SaleId}");

        return Task.CompletedTask;
    }
}
