using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events.Branch.Created;

/// <summary>
/// Event triggered when a branch is created.
/// </summary>
public sealed record BranchCreatedEvent(
    Guid Id,
    string Name)
    : INotification;
