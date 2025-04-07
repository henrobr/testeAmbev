using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Domain.Events.Sale.Deleted;

/// <summary>
/// Event handler responsible for processing the SaleCancelled event.
/// </summary>
public sealed class SaleDeletedEventHandler(
    ILogger<SaleDeletedEventHandler> logger) 
    : INotificationHandler<SaleDeletedEvent>
{
    /// <summary>
    /// Handles the SaleCancelled event by logging the sale cancellation.
    /// </summary>
    public Task Handle(SaleDeletedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Sale cancelled: ID {notification.SaleId}");

        return Task.CompletedTask;
    }
}
