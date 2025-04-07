using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.Commands.Create;

/// <summary>
/// Represents a command to create a new product.
/// </summary>
/// <param name="Name">The name of the product to be created.</param>
/// <param name="Price">The price of the product to be created.</param>
public sealed record CreateProductCommand(string Name, decimal Price) : IRequest<int>;