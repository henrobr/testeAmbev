using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Domain.Events.Branch.Created;

/// <summary>
/// Event handler responsible for processing the BranchCreated event.
/// </summary>
public sealed class BranchCreatedEventHandler(
    ILogger<BranchCreatedEventHandler> logger)
    : INotificationHandler<BranchCreatedEvent>
{

    /// <summary>
    /// Handles the BranchCreated event by logging the sale details.
    /// </summary>
    /// <param name="notification">BranchCreated event data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public Task Handle(BranchCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Branch created: ID {notification.Id} " +
            $"| Name {notification.Name} ");

        return Task.CompletedTask;
    }
}
