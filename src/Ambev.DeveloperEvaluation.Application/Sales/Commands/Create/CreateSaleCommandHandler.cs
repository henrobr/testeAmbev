using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Events.Sale.Created;
using Ambev.DeveloperEvaluation.Domain.Models.BranchAggregate.Repositories;
using Ambev.DeveloperEvaluation.Domain.Models.CustomerAggregate.Repositories;
using Ambev.DeveloperEvaluation.Domain.Models.ProductAggregate.Repositories;
using Ambev.DeveloperEvaluation.Domain.Models.SaleAggregate.Entities;
using Ambev.DeveloperEvaluation.Domain.Models.SaleAggregate.Repositories;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.Create;

/// <summary>
/// Handles the creation of a new sale, ensuring that all rules and validations are applied.
/// </summary>
public sealed class CreateSaleCommandHandler(
    IProductRepository productRepository,
    ISaleRepository saleRepository,
    IBranchRepository branchRepository,
    ICustomerRepository customerRepository,
    IUnitOfWork unitOfWork,
    IMediator mediator)
    : IRequestHandler<CreateSaleCommand, int>
{
    /// <summary>
    /// Processes the sale creation command.
    /// </summary>
    /// <param name="request">The sale request data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created sale details.</returns>
    /// <exception cref="ValidationException">Thrown when validation fails.</exception>
    /// <exception cref="InvalidOperationException">Thrown when a referenced product does not exist.</exception>
    public async Task<int> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateSaleValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        var failures = new List<ValidationFailure>();

        if (!validationResult.IsValid)
            failures.AddRange(validationResult.Errors);

        var customer = await customerRepository
            .GetByIdAsNoTrackingAsync(request.CustomerId, cancellationToken);
        if (customer is null)
            failures.Add(new ValidationFailure("CustomerId", 
                "The specified customer does not exist"));

        var branch = await branchRepository
            .GetByIdAsNoTrackingAsync(request.BranchId, cancellationToken);
        if (branch is null)
            failures.Add(new ValidationFailure("BranchId", 
                "The specified branch does not exist"));

        var sale = Sale.Create(request.CustomerId, request.BranchId);

        foreach (var item in request.Items)
        {
            var product = await productRepository.GetByIdAsNoTrackingAsync(item.ProductId, 
                cancellationToken);
            if (product is null)
            {
                failures.Add(new ValidationFailure($"ProductId: {item.ProductId}", 
                    $"Product with ID: {item.ProductId} was not found"));
                continue;
            }

            sale.AddItem(
                product.Id,
                item.Quantity,
                product.Price);
        }

        if (failures.Any())
            throw new ValidationException(failures);

        await saleRepository.CreateAsync(sale, cancellationToken);

        if (!await unitOfWork.CommitAsync(cancellationToken))
            throw new ValidationException("Sale not created");

        await mediator.Publish(new SaleCreatedEvent(
            sale.Id,
            sale.CustomerId,
            sale.BranchId,
            sale.TotalAmount),
            cancellationToken);

        return sale.Id;
    }
}