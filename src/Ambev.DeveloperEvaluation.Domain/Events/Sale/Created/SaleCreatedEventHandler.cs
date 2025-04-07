using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Domain.Events.Sale.Created;

/// <summary>
/// Event handler responsible for processing the SaleCreated event.
/// </summary>
public sealed class SaleCreatedEventHandler(
    ILogger<SaleCreatedEventHandler> logger)
    : INotificationHandler<SaleCreatedEvent>
{

    /// <summary>
    /// Handles the SaleCreated event by logging the sale details.
    /// </summary>
    /// <param name="notification">SaleCreated event data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public Task Handle(SaleCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Sale created: ID {notification.SaleId} " +
            $"| Customer {notification.CustomerId} " +
            $"| Branch {notification.BranchId} " +
            $"| Total Amount $ {notification.TotalAmount:F2}");

        return Task.CompletedTask;
    }
}
