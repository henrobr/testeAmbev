using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Domain.Events.Customer.Created;

/// <summary>
/// Event handler responsible for processing the CustomerCreated event.
/// </summary>
public sealed class CustomerCreatedEventHandler(
    ILogger<CustomerCreatedEventHandler> logger)
    : INotificationHandler<CustomerCreatedEvent>
{

    /// <summary>
    /// Handles the CustomerCreated event by logging the sale details.
    /// </summary>
    /// <param name="notification">CustomerCreated event data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public Task Handle(CustomerCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Customer created: ID {notification.Id} " +
            $"| Name {notification.Name} ");

        return Task.CompletedTask;
    }
}
