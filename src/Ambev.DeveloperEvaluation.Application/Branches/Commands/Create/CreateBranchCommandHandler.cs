using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Events.Branch.Created;
using Ambev.DeveloperEvaluation.Domain.Models.BranchAggregate.Entities;
using Ambev.DeveloperEvaluation.Domain.Models.BranchAggregate.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.Commands.Create;

/// <summary>
/// Handles the creation of a new branch.
/// </summary>
/// <param name="branchRepository">Repository for performing operations related to branches.</param>
/// <param name="unitOfWork">Unit of work to commit changes to the data store.</param>
/// <param name="mediator">Mediator for publishing domain events.</param>
public sealed class CreateBranchCommandHandler(
    IBranchRepository branchRepository,
    IUnitOfWork unitOfWork,
    IMediator mediator)
    : IRequestHandler<CreateBranchCommand, Guid>
{
    /// <summary>
    /// Handles the <see cref="CreateBranchCommand"/> by validating input, creating a branch,
    /// persisting it, and publishing a <see cref="BranchCreatedEvent"/>.
    /// </summary>
    /// <param name="request">The command containing the data required to create the branch.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The ID of the newly created branch.</returns>
    /// <exception cref="ValidationException">
    /// Thrown if validation fails or if the branch could not be created in the data store.
    /// </exception>
    public async Task<Guid> Handle(CreateBranchCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateBranchValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var branch = Branch.Create(request.Name);

        await branchRepository.CreateAsync(branch, cancellationToken);

        if (!await unitOfWork.CommitAsync(cancellationToken))
            throw new ValidationException("Branch not created");

        await mediator.Publish(new BranchCreatedEvent(
            branch.Id,
            branch.Name),
            cancellationToken);

        return branch.Id;
    }
}

