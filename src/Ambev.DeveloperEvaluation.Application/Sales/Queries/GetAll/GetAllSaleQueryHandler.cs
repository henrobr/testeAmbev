using Ambev.DeveloperEvaluation.Domain.Models.SaleAggregate.DTOs;
using Ambev.DeveloperEvaluation.Domain.Models.SaleAggregate.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries.GetListAll;

/// <summary>
/// Handles the <see cref="GetAllSaleQuery"/> to retrieve a list of sales with optional filters.
/// </summary>
/// <param name="saleRepository">Repository interface used to access sales data.</param>
public sealed class GetAllSaleQueryHandler(ISaleRepository saleRepository)
    : IRequestHandler<GetAllSaleQuery, IQueryable<SaleDto>>
{
    /// <summary>
    /// Handles the request to retrieve sales based on optional filter criteria.
    /// </summary>
    /// <param name="request">The query containing optional filters: SaleId, CustomerName, and BranchName.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>An <see cref="IQueryable{SaleDto}"/> representing the filtered list of sales.</returns>
    public async Task<IQueryable<SaleDto>> Handle(GetAllSaleQuery request, CancellationToken cancellationToken)
    {
        return saleRepository
            .GetAllAsNoTracking(request.SaleId, request.CustomerName, request.BranchName);
    }
}
