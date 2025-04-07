using Ambev.DeveloperEvaluation.Application.Sales.Queries.GetById;
using Ambev.DeveloperEvaluation.Domain.Models.SaleAggregate.DTOs;
using Ambev.DeveloperEvaluation.Domain.Models.SaleAggregate.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

/// <summary>
/// Unit tests for the <see cref="GetByIdQueryHandler"/>.
/// </summary>
public class GetSaleByIdQueryHandlerTests
{
    private readonly ISaleRepository _saleRepository = Substitute.For<ISaleRepository>();
    private readonly GetByIdQueryHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetSaleByIdQueryHandlerTests"/> class.
    /// </summary>
    public GetSaleByIdQueryHandlerTests()
    {
        _handler = new GetByIdQueryHandler(_saleRepository);
    }

    /// <summary>
    /// Ensures that a valid sale ID returns the corresponding <see cref="SaleDto"/>.
    /// </summary>
    [Fact]
    public async Task Handle_ValidSaleId_ReturnsSaleDto()
    {
        // Arrange
        var saleId = 1;
        var expectedDto = GetSaleByIdQueryTestData.CreateValidSaleDto(saleId);

        _saleRepository.GetByIdAsNoTrackingAsync(saleId, Arg.Any<CancellationToken>())
            .Returns(expectedDto);

        var query = new GetByIdQuery(saleId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedDto);
    }

    /// <summary>
    /// Ensures that an invalid sale ID throws a validation exception.
    /// </summary>
    [Fact]
    public async Task Handle_InvalidSaleId_ThrowsValidationException()
    {
        // Arrange
        var saleId = 999;
        _saleRepository.GetByIdAsNoTrackingAsync(saleId, Arg.Any<CancellationToken>())
            .Returns((SaleDto?)null);

        var query = new GetByIdQuery(saleId);

        // Act
        var act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("Sale not found");
    }
}
