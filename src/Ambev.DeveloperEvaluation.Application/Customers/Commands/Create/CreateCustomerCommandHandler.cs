using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Events.Customer.Created;
using Ambev.DeveloperEvaluation.Domain.Models.CustomerAggregate.Entities;
using Ambev.DeveloperEvaluation.Domain.Models.CustomerAggregate.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Customers.Commands.Create;

/// <summary>
/// Handles the creation of a new customer.
/// </summary>
/// <param name="customerRepository">Repository for performing operations related to customers.</param>
/// <param name="unitOfWork">Unit of work to commit changes to the data store.</param>
/// <param name="mediator">Mediator for publishing domain events.</param>
public sealed class CreateCustomerCommandHandler(
    ICustomerRepository customerRepository,
    IUnitOfWork unitOfWork,
    IMediator mediator)
    : IRequestHandler<CreateCustomerCommand, Guid>
{
    /// <summary>
    /// Handles the <see cref="CreateCustomerCommand"/> by validating input, creating a customer,
    /// persisting it, and publishing a <see cref="CustomerCreatedEvent"/>.
    /// </summary>
    /// <param name="request">The command containing the data required to create the customer.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The ID of the newly created customer.</returns>
    /// <exception cref="ValidationException">
    /// Thrown if validation fails or if the customer could not be created in the data store.
    /// </exception>
    public async Task<Guid> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateCustomerValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var customer = Customer.Create(request.Name);

        await customerRepository.CreateAsync(customer, cancellationToken);

        if (!await unitOfWork.CommitAsync(cancellationToken))
            throw new ValidationException("Customer not created");

        await mediator.Publish(new CustomerCreatedEvent(
            customer.Id,
            customer.Name),
            cancellationToken);

        return customer.Id;
    }
}

