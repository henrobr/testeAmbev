using Ambev.DeveloperEvaluation.Domain.Models.SaleAggregate.DTOs;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries.GetListAll;

/// <summary>
/// Query object for retrieving a list of sales with optional filtering parameters.
/// </summary>
/// <param name="SaleId">Optional. Filters the result by the specific sale ID.</param>
/// <param name="CustomerName">Optional. Filters the result to include only sales where the customer's name contains the specified value.</param>
/// <param name="BranchName">Optional. Filters the result to include only sales where the branch name contains the specified value.</param>
/// <remarks>
/// This query is used in the CQRS pattern and is handled via MediatR to return a list of <see cref="SaleDto"/>.
/// </remarks>
public sealed record GetAllSaleQuery(
    int? SaleId = null,
    string? CustomerName = null,
    string? BranchName = null)
    : IRequest<IQueryable<SaleDto>>;