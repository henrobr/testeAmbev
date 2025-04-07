using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Events.Sale.Cancelled;
using Ambev.DeveloperEvaluation.Domain.Models.SaleAggregate.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.Cancel;

/// <summary>
/// Handles the cancellation of a sale.
/// Validates if the sale exists and is not already cancelled.
/// If the cancellation is successful, an event is published.
/// </summary>
public sealed class CancelSaleCommandHandler(
    ISaleRepository saleRepository,
    IUnitOfWork unitOfWork,
    IMediator mediator)
    : IRequestHandler<CancelSaleCommand, bool>
{
    /// <summary>
    /// Handles the cancellation of a sale.
    /// </summary>
    /// <param name="request">The cancellation command containing the sale ID.</param>
    /// <param name="cancellationToken">Token for handling operation cancellation.</param>
    /// <returns>True if the sale was successfully cancelled, false otherwise.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the sale does not exist.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the sale is already cancelled.</exception>
    public async Task<bool> Handle(CancelSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = await saleRepository.GetByIdAsync(request.SaleId, cancellationToken)
            ?? throw new KeyNotFoundException("Sale not found");

        if (sale.Status == SaleStatus.Cancelled)
            throw new InvalidOperationException("This sale has already been cancelled");

        sale.Cancel();

        if (!await unitOfWork.CommitAsync(cancellationToken))
            return false;

        await mediator.Publish(new SaleCancelledEvent(sale.Id), cancellationToken);

        return true;
    }
}
