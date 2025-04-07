using Ambev.DeveloperEvaluation.Domain.Models.SaleAggregate.DTOs;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries.GetById;

public sealed record GetByIdQuery(int SaleId) : IRequest<SaleDto>;