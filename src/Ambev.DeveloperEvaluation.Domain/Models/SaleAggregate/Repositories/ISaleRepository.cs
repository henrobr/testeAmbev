using Ambev.DeveloperEvaluation.Domain.Models.SaleAggregate.DTOs;
using Ambev.DeveloperEvaluation.Domain.Models.SaleAggregate.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Models.SaleAggregate.Repositories;

public interface ISaleRepository
{
    Task CreateAsync(Sale sale, CancellationToken cancellation);

    Task<Sale> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<SaleDto?> GetByIdAsNoTrackingAsync(int id, CancellationToken cancellationToken);

    IQueryable<SaleDto> GetAllAsNoTracking(
        int? saleId = null,
        string? customerName = null,
        string? branchName = null);

    void Remove(Sale sale);
}
