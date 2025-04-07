using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events.Customer.Created;

/// <summary>
/// Event triggered when a customer is created.
/// </summary>
public sealed record CustomerCreatedEvent(
    Guid Id,
    string Name)
    : INotification;
