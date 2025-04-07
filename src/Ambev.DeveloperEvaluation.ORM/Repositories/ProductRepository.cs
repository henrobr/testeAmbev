using Ambev.DeveloperEvaluation.Domain.Models.ProductAggregate.DTOs;
using Ambev.DeveloperEvaluation.Domain.Models.ProductAggregate.Entities;
using Ambev.DeveloperEvaluation.Domain.Models.ProductAggregate.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Repository implementation for managing <see cref="Product"/> entities.
/// Provides methods to create and retrieve products from the database.
/// </summary>
public class ProductRepository(DefaultContext context) : IProductRepository
{
    private readonly DbSet<Product> _products = context.Set<Product>();

    /// <summary>
    /// Asynchronously adds a new product to the database.
    /// </summary>
    /// <param name="product">The product entity to be added.</param>
    /// <param name="cancellationToken">Token to observe while waiting for the operation to complete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task CreateAsync(Product product, CancellationToken cancellationToken)
    {
        await _products.AddAsync(product, cancellationToken);
    }

    /// <summary>
    /// Retrieves a product by its ID asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the product.</param>
    /// <param name="cancellationToken">Token to observe while waiting for the operation to complete.</param>
    /// <returns>
    /// The product entity if found; otherwise, null.
    /// </returns>
    public async Task<Product> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _products.SingleOrDefaultAsync(f => f.Id == id, cancellationToken);
    }

    /// <summary>
    /// Retrieves a product by its ID asynchronously with no tracking.
    /// Useful for read-only operations to improve performance.
    /// </summary>
    /// <param name="id">The unique identifier of the product.</param>
    /// <param name="cancellationToken">Token to observe while waiting for the operation to complete.</param>
    /// <returns>
    /// The product entity if found; otherwise, null.
    /// </returns>
    public async Task<Product> GetByIdAsNoTrackingAsync(int id, 
        CancellationToken cancellationToken)
    {
        return await _products.AsNoTracking()
            .SingleOrDefaultAsync(f => f.Id == id, cancellationToken);
    }

    public async Task<List<Product>> GetByIdsAsNoTrackingAsync(IEnumerable<int> ids,
        CancellationToken cancellationToken)
    {
        return await _products
            .Where(p => ids.Contains(p.Id))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Retrieves a queryable list of products without tracking, optionally filtered by name.
    /// </summary>
    /// <param name="name">Optional filter to search products by name. If null or empty, all products are returned.</param>
    /// <returns>
    /// An <see cref="IQueryable{ProductDto}"/> representing the list of products with no entity tracking.
    /// </returns>
    public IQueryable<ProductDto> GetAllAsNoTracking(string? name = null)
    {
        var query = from product in _products.AsNoTracking()
                    select new ProductDto(product.Id, product.Name, product.Price);

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(x => x.Name.Contains(name));

        return query.AsQueryable();
    }
}
