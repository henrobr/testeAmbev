using Ambev.DeveloperEvaluation.Domain.Models.BranchAggregate.Entities;
using Ambev.DeveloperEvaluation.Domain.Models.CustomerAggregate.Entities;
using Ambev.DeveloperEvaluation.Domain.Models.ProductAggregate.Entities;
using Ambev.DeveloperEvaluation.Domain.Models.SaleAggregate.DTOs;
using Ambev.DeveloperEvaluation.Domain.Models.SaleAggregate.Entities;
using Ambev.DeveloperEvaluation.Domain.Models.SaleAggregate.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Repository implementation for managing <see cref="Sale"/> entities.
/// Provides methods to create and retrieve sales records from the database.
/// </summary>
public class SaleRepository(DefaultContext context)
    : ISaleRepository
{
    private readonly DbSet<Sale> _sales = context.Set<Sale>();

    /// <summary>
    /// Asynchronously adds a new sale to the database.
    /// </summary>
    /// <param name="sale">The sale entity to be added.</param>
    /// <param name="cancellation">Token to observe while waiting for the operation to complete. Defaults to <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task CreateAsync(Sale sale, CancellationToken cancellation)
    {
        await _sales.AddAsync(sale, cancellation);
    }

    /// <summary>
    /// Retrieves a sale by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the sale.</param>
    /// <param name="cancellationToken">Token to observe while waiting for the operation to complete. Defaults to <see cref="CancellationToken.None"/>.</param>
    /// <returns>The sale entity if found; otherwise, null.</returns>
    public async Task<Sale> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _sales.FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
    }

    /// <summary>
    /// Retrieves a single sale by its ID with no entity tracking, including customer, branch,
    /// and associated sale items with product details.
    /// </summary>
    /// <param name="id">The unique identifier of the sale to retrieve.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A <see cref="SaleDto"/> representing the sale and its details if found; otherwise, <c>null</c>.
    /// </returns>
    public async Task<SaleDto?> GetByIdAsNoTrackingAsync(int id, CancellationToken cancellationToken)
    {
        var query = from sale in _sales.AsNoTracking()
                    join customer in context.QueryAsNoTracking<Customer>()
                        on sale.CustomerId equals customer.Id
                    join branch in context.QueryAsNoTracking<Branch>()
                        on sale.BranchId equals branch.Id
                    join item in context.QueryAsNoTracking<SaleItem>()
                        on sale.Id equals item.SaleId
                    join product in context.QueryAsNoTracking<Product>()
                        on item.ProductId equals product.Id
                    where sale.Id == id
                    select new
                    {
                        Sale = sale,
                        CustomerName = customer.Name,
                        BranchName = branch.Name,
                        Item = new SaleItemDto(
                            item.Id,
                            item.SaleId,
                            item.ProductId,
                            product.Name,
                            item.Quantity,
                            item.UnitPrice,
                            item.Discount)
                    };

        var result = await query.ToListAsync(cancellationToken);

        if (!result.Any())
            return null;

        var saleDto = result
            .GroupBy(x => x.Sale.Id)
            .Select(g => new SaleDto(
                g.Key,
                (int)g.First().Sale.Status,
                g.First().Sale.Status.ToString(),
                g.First().Sale.CustomerId,
                g.First().CustomerName,
                g.First().Sale.BranchId,
                g.First().BranchName,
                g.Select(x => x.Item).ToList()
            ))
            .SingleOrDefault();

        return saleDto;
    }

    /// <summary>
    /// Retrieves a queryable list of <see cref="SaleDto"/> objects with optional filters applied.
    /// </summary>
    /// <param name="saleId">Optional. Filters the results to include only the sale with the specified ID.</param>
    /// <param name="customerName">Optional. Filters the results to include only sales where the customer's name contains the specified value.</param>
    /// <param name="branchName">Optional. Filters the results to include only sales where the branch name contains the specified value.</param>
    /// <returns>
    /// An <see cref="IQueryable{SaleDto}"/> representing sales with related customer, branch, and product data. 
    /// Each sale includes its sale items and filtered based on the provided parameters, if any.
    /// </returns>
    /// <remarks>
    /// This method uses <c>AsNoTracking</c> to improve performance by disabling change tracking for read-only scenarios. 
    /// The result is grouped by sale ID and projected into <see cref="SaleDto"/> objects with nested <see cref="SaleItemDto"/> lists.
    /// </remarks>

    public IQueryable<SaleDto> GetAllAsNoTracking(
        int? saleId = null,
        string? customerName = null,
        string? branchName = null)
    {
        var query = from sale in _sales.AsNoTracking()
                    join customer in context.QueryAsNoTracking<Customer>()
                        on sale.CustomerId equals customer.Id
                    join branch in context.QueryAsNoTracking<Branch>()
                        on sale.BranchId equals branch.Id
                    join item in context.QueryAsNoTracking<SaleItem>()
                        on sale.Id equals item.SaleId
                    join product in context.QueryAsNoTracking<Product>()
                        on item.ProductId equals product.Id
                    select new
                    {
                        Sale = sale,
                        CustomerName = customer.Name,
                        BranchName = branch.Name,
                        Item = new SaleItemDto(
                            item.Id,
                            item.SaleId,
                            item.ProductId,
                            product.Name,
                            item.Quantity,
                            item.UnitPrice,
                            item.Discount)
                    };

        if (saleId.HasValue)
            query = query.Where(x => x.Sale.Id == saleId.Value);

        if (!string.IsNullOrWhiteSpace(customerName))
            query = query.Where(x => x.CustomerName.Contains(customerName));

        if (!string.IsNullOrWhiteSpace(branchName))
            query = query.Where(x => x.BranchName.Contains(branchName));

        var groupedQuery = query
            .GroupBy(x => x.Sale.Id)
            .Select(g => new SaleDto(
                g.Key,
                (int)g.First().Sale.Status,
                g.First().Sale.Status.ToString(),
                g.First().Sale.CustomerId,
                g.First().CustomerName,
                g.First().Sale.BranchId,
                g.First().BranchName,
                g.Select(x => x.Item).ToList()
            ));

        return groupedQuery;
    }

    /// <summary>
    /// Removes the specified sale from the collection of sales.
    /// </summary>
    /// <param name="sale">The sale to be removed.</param>
    public void Remove(Sale sale)
    {
        _sales.Remove(sale);
    }
}
