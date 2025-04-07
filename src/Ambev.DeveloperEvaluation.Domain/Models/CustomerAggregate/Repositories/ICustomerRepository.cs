using Ambev.DeveloperEvaluation.Domain.Models.CustomerAggregate.DTOs;
using Ambev.DeveloperEvaluation.Domain.Models.CustomerAggregate.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Models.CustomerAggregate.Repositories;

public interface ICustomerRepository
{
    Task CreateAsync(Customer branch, CancellationToken cancellationToken);
    Task<Customer> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Customer> GetByIdAsNoTrackingAsync(Guid id,
        CancellationToken cancellationToken);
    IQueryable<CustomerDto> GetAllAsNoTracking(string? name = null);
}
