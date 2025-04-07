using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Domain.Events.Sale.Modified;

/// <summary>
/// Event handler responsible for processing the SaleModified event.
/// </summary>
public sealed class SaleModifiedEventHandler(
    ILogger<SaleModifiedEventHandler> logger)
    : INotificationHandler<SaleModifiedEvent>
{
    /// <summary>
    /// Handles the SaleModified event by logging the sale details.
    /// </summary>
    public Task Handle(SaleModifiedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Sale modified: ID {notification.SaleId} " +
            $"| Customer {notification.CustomerId} " +
            $"| Branch {notification.BranchId} " +
            $"| Total Amount $ {notification.TotalAmount:F2}");

        return Task.CompletedTask;
    }
}
