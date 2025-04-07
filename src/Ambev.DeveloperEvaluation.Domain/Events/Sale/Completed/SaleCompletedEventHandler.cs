using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Domain.Events.Sale.Completed;

/// <summary>
/// Event handler responsible for processing the SaleCompleted event.
/// </summary>
public sealed class SaleCompletedEventHandler(
    ILogger<SaleCompletedEventHandler> logger)
    : INotificationHandler<SaleCompletedEvent>
{

    /// <summary>
    /// Handles the SaleCompleted event by logging the sale details.
    /// </summary>
    /// <param name="notification">SaleCompleted event data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public Task Handle(SaleCompletedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Sale completed: ID {notification.SaleId}");

        return Task.CompletedTask;
    }
}
