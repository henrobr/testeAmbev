using Ambev.DeveloperEvaluation.Domain.Models.CustomerAggregate.DTOs;

namespace Ambev.DeveloperEvaluation.Unit.Application.Costumers.TestData;

/// <summary>
/// Provides test data for <see cref="GetAllCustomerQueryHandlerTests"/>.
/// </summary>
public static class GetAllCustomerQueryHandlerTestData
{
    /// <summary>
    /// Generates a full list of customer DTOs for testing purposes.
    /// </summary>
    /// <returns>An <see cref="IQueryable{CustomerDto}"/> representing all customers.</returns>
    public static IQueryable<CustomerDto> GenerateAllCustomers()
    {
        return new List<CustomerDto>
        {
            new(Guid.NewGuid(), "JOÃO DA SILVA"),
            new(Guid.NewGuid(), "MARIA DE SOUZA"),
            new(Guid.NewGuid(), "ANA CAROLINA")
        }.AsQueryable();
    }

    /// <summary>
    /// Filters the full customer list by the given name.
    /// </summary>
    /// <param name="name">The name filter to apply.</param>
    /// <returns>A filtered <see cref="IQueryable{CustomerDto}"/>.</returns>
    public static IQueryable<CustomerDto> GenerateFilteredCustomers(string name)
    {
        return GenerateAllCustomers()
            .Where(c => c.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
            .AsQueryable();
    }
}
