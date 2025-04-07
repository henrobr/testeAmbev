using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Events.Sale.Deleted;
using Ambev.DeveloperEvaluation.Domain.Models.SaleAggregate.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.Delete;

/// <summary>
/// Handles the update of a sale, ensuring the necessary validations 
/// and applying the requested changes.
/// </summary>
public sealed class DeleteSaleCommandHandler(
    ISaleRepository saleRepository,
    IUnitOfWork unitOfWork,
    IMediator mediator)
    : IRequestHandler<DeleteSaleCommand, bool>
{
    /// <summary>
    /// Processes the update request for a sale.
    /// </summary>
    /// <param name="request">The update command containing sale details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Returns <c>true</c> if the update is successful; otherwise, <c>false</c>.</returns>
    /// <exception cref="ValidationException">Thrown when validation fails or required data is missing.</exception>
    public async Task<bool> Handle(DeleteSaleCommand request, CancellationToken cancellationToken)
    {
        var validator = new DeleteSaleValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var sale = await saleRepository.GetByIdAsync(request.SaleId, cancellationToken)
            ?? throw new ValidationException("Sale not found");

        saleRepository.Remove(sale);

        if (!await unitOfWork.CommitAsync(cancellationToken))
            return false;

        await mediator.Publish(new SaleDeletedEvent(sale.Id), cancellationToken);

        return true;
    }
}
