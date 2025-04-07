using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Events.Sale.Modified;
using Ambev.DeveloperEvaluation.Domain.Models.BranchAggregate.Repositories;
using Ambev.DeveloperEvaluation.Domain.Models.CustomerAggregate.Repositories;
using Ambev.DeveloperEvaluation.Domain.Models.ProductAggregate.Repositories;
using Ambev.DeveloperEvaluation.Domain.Models.SaleAggregate.Entities;
using Ambev.DeveloperEvaluation.Domain.Models.SaleAggregate.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.Update;

/// <summary>
/// Handles the update of a sale, ensuring the necessary validations 
/// and applying the requested changes.
/// </summary>
public sealed class UpdateSaleCommandHandler(
    ISaleRepository saleRepository,
    ICustomerRepository customerRepository,
    IBranchRepository branchRepository,
    IProductRepository productRepository,
    IUnitOfWork unitOfWork,
    IMediator mediator)
    : IRequestHandler<UpdateSaleCommand, bool>
{
    /// <summary>
    /// Processes the update request for a sale.
    /// </summary>
    /// <param name="request">The update command containing sale details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Returns <c>true</c> if the update is successful; otherwise, <c>false</c>.</returns>
    /// <exception cref="ValidationException">Thrown when validation fails or required data is missing.</exception>
    public async Task<bool> Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateSaleValidator(request.SaleId);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var sale = await saleRepository.GetByIdAsync(request.SaleId, cancellationToken)
            ?? throw new ValidationException("Sale not found");

        if (sale.Status is not SaleStatus.Pending)
            throw new ValidationException("The sale can only be updated if it has a 'Pending' status");

        await UpdateCustomerOrBranchIfNeededAsync(
            sale, 
            request.BranchId, 
            request.CustomerId, 
            cancellationToken);

        await UpdateSaleItemsIfNeededAsync(sale, request.Items, cancellationToken);

        if (!await unitOfWork.CommitAsync(cancellationToken))
            throw new ValidationException("Sale not updated");

        await mediator.Publish(new SaleModifiedEvent(
            sale.Id,
            sale.CustomerId,
            sale.BranchId,
            sale.TotalAmount),
            cancellationToken);

        return true;
    }

    #region Private Methods

    /// <summary>
    /// Updates the customer and branch information of the sale if they have changed.
    /// </summary>
    /// <param name="sale">The sale entity being updated.</param>
    /// <param name="branchId">The new branch ID.</param>
    /// <param name="customerId">The new customer ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <exception cref="ValidationException">Thrown if the specified customer or branch does not exist.</exception>
    private async Task UpdateCustomerOrBranchIfNeededAsync(
        Sale sale, Guid branchId, Guid customerId,
        CancellationToken cancellationToken)
    {
        if (sale.CustomerId == customerId) return;

        var customer = await customerRepository.GetByIdAsNoTrackingAsync(customerId, cancellationToken)
            ?? throw new ValidationException("The specified customer does not exist.");

        sale.UpdateCustomer(customer.Id);

        if (sale.BranchId == branchId) return;

        var branch = await branchRepository.GetByIdAsNoTrackingAsync(branchId, cancellationToken)
            ?? throw new ValidationException("The specified branch does not exist.");

        sale.UpdateBranch(branch.Id);
    }

    /// <summary>
    /// Updates the items in the sale if there are changes in the request.
    /// </summary>
    /// <param name="sale">The sale entity being updated.</param>
    /// <param name="items">The new list of sale items.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <exception cref="InvalidOperationException">Thrown if a product in the request is not found.</exception>
    private async Task UpdateSaleItemsIfNeededAsync(
        Sale sale, List<SaleItems>? items,
        CancellationToken cancellationToken)
    {
        if (items is null || !items.Any()) return;

        var productIds = items.Select(i => i.ProductId).ToList();
        var products = await productRepository
            .GetByIdsAsNoTrackingAsync(productIds, cancellationToken);
        var productDict = products.ToDictionary(p => p.Id, p => p);

        sale.ClearItems();

        foreach (var item in items)
        {
            var product = productDict.GetValueOrDefault(item.ProductId)
                ?? throw new InvalidOperationException($"Product with ID: {item.ProductId} was not found");

            sale.AddItem(product.Id, item.Quantity, product.Price);
        }
    }
    
    #endregion Private Methods
}
