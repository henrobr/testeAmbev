using Ambev.DeveloperEvaluation.Domain.Models.BranchAggregate.DTOs;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.Queries.GetAll;

/// <summary>
/// Query to retrieve all branches, optionally filtered by name.
/// </summary>
/// <param name="Name">Optional name to filter the branches by.</param>
public sealed record GetAllBranchQuery(string? Name = null) : IRequest<IQueryable<BranchDto>>;