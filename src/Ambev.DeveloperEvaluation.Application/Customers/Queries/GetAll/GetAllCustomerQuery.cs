using Ambev.DeveloperEvaluation.Domain.Models.CustomerAggregate.DTOs;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Customers.Queries.GetAll;

/// <summary>
/// Query to retrieve all customers, optionally filtered by name.
/// </summary>
/// <param name="Name">Optional name filter to search for customers by name.</param>
public sealed record GetAllCustomerQuery(string? Name = null) : IRequest<IQueryable<CustomerDto>>;