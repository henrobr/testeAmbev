using Ambev.DeveloperEvaluation.Domain.Models.CustomerAggregate.DTOs;
using Ambev.DeveloperEvaluation.Domain.Models.CustomerAggregate.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Customers.Queries.GetAll;

/// <summary>
/// Handles the <see cref="GetAllCustomerQuery"/> to retrieve a list of customers,
/// optionally filtered by name, without tracking changes.
/// </summary>
/// <param name="customerRepository">The repository used to access customer data.</param>
public sealed class GetAllCustomerQueryHandler(ICustomerRepository customerRepository)
    : IRequestHandler<GetAllCustomerQuery, IQueryable<CustomerDto>>
{
    /// <inheritdoc />
    public async Task<IQueryable<CustomerDto>> Handle(GetAllCustomerQuery request, 
        CancellationToken cancellationToken)
    {
        return customerRepository.GetAllAsNoTracking(request.Name);
    }
}