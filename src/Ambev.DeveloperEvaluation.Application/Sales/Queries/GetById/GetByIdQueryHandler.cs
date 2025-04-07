using Ambev.DeveloperEvaluation.Domain.Models.SaleAggregate.DTOs;
using Ambev.DeveloperEvaluation.Domain.Models.SaleAggregate.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries.GetById;

public sealed class GetByIdQueryHandler(ISaleRepository saleRepository)
    : IRequestHandler<GetByIdQuery, SaleDto>
{
    public async Task<SaleDto> Handle(GetByIdQuery request, CancellationToken cancellationToken)
    {
        return await saleRepository.GetByIdAsNoTrackingAsync(request.SaleId, cancellationToken)
            ?? throw new ValidationException("Sale not found");
    }
}
