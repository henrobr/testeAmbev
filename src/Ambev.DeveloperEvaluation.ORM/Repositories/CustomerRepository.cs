using Ambev.DeveloperEvaluation.Domain.Models.CustomerAggregate.DTOs;
using Ambev.DeveloperEvaluation.Domain.Models.CustomerAggregate.Entities;
using Ambev.DeveloperEvaluation.Domain.Models.CustomerAggregate.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Repository implementation for managing <see cref="Customer"/> entities.
/// Provides methods to create and retrieve customer records from the database.
/// </summary>
public class CustomerRepository(DefaultContext context) : ICustomerRepository
{
    private readonly DbSet<Customer> _customers = context.Set<Customer>();

    /// <summary>
    /// Asynchronously adds a new customer to the database.
    /// </summary>
    /// <param name="customer">The customer entity to be added.</param>
    /// <param name="cancellationToken">Token to observe while waiting for the operation to complete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task CreateAsync(Customer customer, CancellationToken cancellationToken)
    {
        await _customers.AddAsync(customer, cancellationToken);
    }

    /// <summary>
    /// Retrieves a customer by their unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the customer.</param>
    /// <param name="cancellationToken">Token to observe while waiting for the operation to complete.</param>
    /// <returns>
    /// The customer entity if found; otherwise, null.
    /// </returns>
    public async Task<Customer> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _customers.SingleOrDefaultAsync(f => f.Id == id, cancellationToken);
    }

    /// <summary>
    /// Retrieves a customer by their unique identifier asynchronously with no tracking.
    /// Useful for read-only operations to improve performance.
    /// </summary>
    /// <param name="id">The unique identifier of the customer.</param>
    /// <param name="cancellationToken">Token to observe while waiting for the operation to complete.</param>
    /// <returns>
    /// The customer entity if found; otherwise, null.
    /// </returns>
    public async Task<Customer> GetByIdAsNoTrackingAsync(Guid id,
        CancellationToken cancellationToken)
    {
        return await _customers.AsNoTracking()
            .SingleOrDefaultAsync(f => f.Id == id, cancellationToken);
    }

    /// <summary>
    /// Retrieves all customers as a queryable collection without tracking changes, optionally filtering by customer name.
    /// </summary>
    /// <param name="name">Optional name filter. If provided, only customer whose names contain this value will be returned.</param>
    /// <returns>
    /// An <see cref="IQueryable{CustomerDto}"/> containing the list of customer without change tracking,
    /// optionally filtered by name.
    /// </returns>
    public IQueryable<CustomerDto> GetAllAsNoTracking(string? name = null)
    {
        var query = from customer in _customers.AsNoTracking()
                    select new CustomerDto(customer.Id, customer.Name);

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(x => x.Name.Contains(name));

        return query.AsQueryable();
    }
}
