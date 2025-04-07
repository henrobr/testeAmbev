using Ambev.DeveloperEvaluation.Domain.Models.ProductAggregate.DTOs;
using Ambev.DeveloperEvaluation.Domain.Models.ProductAggregate.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Models.ProductAggregate.Repositories;

public interface IProductRepository
{
    Task CreateAsync(Product product, CancellationToken cancellationToken);
    Task<Product> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<Product> GetByIdAsNoTrackingAsync(int id, CancellationToken cancellationToken);
    Task<List<Product>> GetByIdsAsNoTrackingAsync(IEnumerable<int> ids,
        CancellationToken cancellationToken);
    IQueryable<ProductDto> GetAllAsNoTracking(string? name = null);
}
