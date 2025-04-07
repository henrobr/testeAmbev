using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Events.Sale.Cancelled;
using Ambev.DeveloperEvaluation.Domain.Models.SaleAggregate.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.Complete;

/// <summary>
/// Handles the cancellation of a sale.
/// Validates if the sale exists and is not already cancelled.
/// If the cancellation is successful, an event is published.
/// </summary>
public sealed class CompleteSaleCommandHandler(
    ISaleRepository saleRepository,
    IUnitOfWork unitOfWork,
    IMediator mediator)
    : IRequestHandler<CompleteSaleCommand, bool>
{
    /// <summary>
    /// Handles the cancellation of a sale.
    /// </summary>
    /// <param name="request">The cancellation command containing the sale ID.</param>
    /// <param name="cancellationToken">Token for handling operation cancellation.</param>
    /// <returns>True if the sale was successfully cancelled, false otherwise.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the sale does not exist.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the sale is already cancelled.</exception>
    public async Task<bool> Handle(CompleteSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = await saleRepository.GetByIdAsync(request.SaleId, cancellationToken)
            ?? throw new ValidationException("Sale not found");

        if (sale.Status == SaleStatus.Cancelled)
            throw new ValidationException("This sale has already been cancelled");

        sale.Complete();

        if (!await unitOfWork.CommitAsync(cancellationToken))
            return false;

        await mediator.Publish(new SaleCancelledEvent(sale.Id), cancellationToken);

        return true;
    }
}
