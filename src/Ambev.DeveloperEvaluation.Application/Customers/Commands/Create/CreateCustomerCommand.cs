using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Customers.Commands.Create;

/// <summary>
/// Represents a command to create a new customer.
/// </summary>
/// <param name="Name">The name of the customer to be created.</param>
public sealed record CreateCustomerCommand(string Name) : IRequest<Guid>;