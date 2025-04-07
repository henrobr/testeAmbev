using Ambev.DeveloperEvaluation.Application.Customers.Queries.GetAll;
using Ambev.DeveloperEvaluation.Domain.Models.CustomerAggregate.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.Costumers.TestData;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Costumers;

/// <summary>
/// Unit tests for the <see cref="GetAllCustomerQueryHandler"/>.
/// </summary>
public class GetAllCustomerQueryHandlerTests
{
    private readonly ICustomerRepository _customerRepository;
    private readonly GetAllCustomerQueryHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAllCustomerQueryHandlerTests"/> class.
    /// </summary>
    public GetAllCustomerQueryHandlerTests()
    {
        _customerRepository = Substitute.For<ICustomerRepository>();
        _handler = new GetAllCustomerQueryHandler(_customerRepository);
    }

    /// <summary>
    /// Tests that when no filter is provided, all customers are returned.
    /// </summary>
    [Fact(DisplayName = "Given no filter When querying customers Then returns all customers")]
    public async Task Handle_WithoutFilter_ReturnsAllCustomers()
    {
        // Arrange
        var expectedCustomers = GetAllCustomerQueryHandlerTestData.GenerateAllCustomers();
        _customerRepository.GetAllAsNoTracking(null).Returns(expectedCustomers);
        var query = new GetAllCustomerQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedCustomers);
    }

    /// <summary>
    /// Tests that when a name filter is provided, only matching customers are returned.
    /// </summary>
    [Fact(DisplayName = "Given name filter When querying customers Then returns filtered customers")]
    public async Task Handle_WithFilter_ReturnsFilteredCustomers()
    {
        // Arrange
        var filter = "JOÃO";
        var expectedFiltered = GetAllCustomerQueryHandlerTestData.GenerateFilteredCustomers(filter);
        _customerRepository.GetAllAsNoTracking(filter).Returns(expectedFiltered);
        var query = new GetAllCustomerQuery(filter);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedFiltered);
    }
}
