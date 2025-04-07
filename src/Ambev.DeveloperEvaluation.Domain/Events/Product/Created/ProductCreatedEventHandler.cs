using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Domain.Events.Product.Created;

/// <summary>
/// Event handler responsible for processing the ProductCreated event.
/// </summary>
public sealed class ProductCreatedEventHandler(
    ILogger<ProductCreatedEventHandler> logger)
    : INotificationHandler<ProductCreatedEvent>
{

    /// <summary>
    /// Handles the ProductCreated event by logging the sale details.
    /// </summary>
    /// <param name="notification">ProductCreated event data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Product created: ID {notification.Id} " +
            $"| Name {notification.Name} " +
            $"| Price $ {notification.Price:F2}");

        return Task.CompletedTask;
    }
}
