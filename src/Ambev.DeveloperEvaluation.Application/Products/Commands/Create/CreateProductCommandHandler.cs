using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Events.Customer.Created;
using Ambev.DeveloperEvaluation.Domain.Events.Product.Created;
using Ambev.DeveloperEvaluation.Domain.Models.CustomerAggregate.Entities;
using Ambev.DeveloperEvaluation.Domain.Models.CustomerAggregate.Repositories;
using Ambev.DeveloperEvaluation.Domain.Models.ProductAggregate.Entities;
using Ambev.DeveloperEvaluation.Domain.Models.ProductAggregate.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.Commands.Create;

/// <summary>
/// Handles the creation of a new product.
/// </summary>
/// <param name="productRepository">Repository for performing operations related to products.</param>
/// <param name="unitOfWork">Unit of work to commit changes to the data store.</param>
/// <param name="mediator">Mediator for publishing domain events.</param>
public sealed class CreateProductCommandHandler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork,
    IMediator mediator)
    : IRequestHandler<CreateProductCommand, int>
{
    /// <summary>
    /// Handles the <see cref="CreateProductCommand"/> by validating input, creating a product,
    /// persisting it, and publishing a <see cref="ProductCreatedEvent"/>.
    /// </summary>
    /// <param name="request">The command containing the data required to create the product.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The ID of the newly created product.</returns>
    /// <exception cref="ValidationException">
    /// Thrown if validation fails or if the product could not be created in the data store.
    /// </exception>
    public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateProductValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var product = Product.Create(request.Name, request.Price);

        await productRepository.CreateAsync(product, cancellationToken);

        if (!await unitOfWork.CommitAsync(cancellationToken))
            throw new ValidationException("Product not created");

        await mediator.Publish(new ProductCreatedEvent(
            product.Id,
            product.Name,
            product.Price),
            cancellationToken);

        return product.Id;
    }
}

