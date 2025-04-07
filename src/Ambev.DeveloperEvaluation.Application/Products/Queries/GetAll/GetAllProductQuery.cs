using Ambev.DeveloperEvaluation.Domain.Models.ProductAggregate.DTOs;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.Queries.GetAll;

/// <summary>
/// Represents a query to retrieve all products, optionally filtered by name.
/// </summary>
/// <param name="Name">Optional filter to search products by name.</param>
public sealed record GetAllProductQuery(string? Name = null) : IRequest<IQueryable<ProductDto>>;