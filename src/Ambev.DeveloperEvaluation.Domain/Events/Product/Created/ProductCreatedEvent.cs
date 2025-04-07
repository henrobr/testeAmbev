using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events.Product.Created;

/// <summary>
/// Event triggered when a branch is created.
/// </summary>
public sealed record ProductCreatedEvent(
    int Id,
    string Name,
    decimal Price)
    : INotification;
