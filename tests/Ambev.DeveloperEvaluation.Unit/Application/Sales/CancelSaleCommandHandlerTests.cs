using Ambev.DeveloperEvaluation.Application.Sales.Commands.Cancel;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Events.Sale.Cancelled;
using Ambev.DeveloperEvaluation.Domain.Models.SaleAggregate.Entities;
using Ambev.DeveloperEvaluation.Domain.Models.SaleAggregate.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData;
using FluentAssertions;
using MediatR;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

/// <summary>
/// Unit tests for <see cref="CancelSaleCommandHandler"/>.
/// </summary>
public class CancelSaleCommandHandlerTests
{
    private readonly ISaleRepository _saleRepository = Substitute.For<ISaleRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private readonly CancelSaleCommandHandler _handler;

    /// <summary>
    /// Initializes the handler with mocked dependencies.
    /// </summary>
    public CancelSaleCommandHandlerTests()
    {
        _handler = new CancelSaleCommandHandler(_saleRepository, _unitOfWork, _mediator);
    }

    /// <summary>
    /// Ensures that a sale is successfully cancelled.
    /// </summary>
    [Fact(DisplayName = "Given valid sale When cancelling sale Then cancels sale")]
    public async Task Handle_ValidSale_CancelsSale()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var sale = CancelSaleCommandTestData.CreatePendingSale(customerId, branchId);
        var command = CancelSaleCommandTestData.CreateValidCommand(sale.Id);

        _saleRepository.GetByIdAsync(sale.Id, Arg.Any<CancellationToken>())
            .Returns(sale);

        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        sale.Status.Should().Be(SaleStatus.Cancelled);
        await _mediator.Received(1)
            .Publish(Arg.Is<SaleCancelledEvent>(e => e.SaleId == sale.Id), 
                Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Ensures that an exception is thrown when trying to cancel a non-existent sale.
    /// </summary>
    [Fact(DisplayName = "Given non-existent sale When cancelling sale Then throws not found exception")]
    public async Task Handle_NonExistentSale_ThrowsKeyNotFoundException()
    {
        // Arrange
        var command = CancelSaleCommandTestData.CreateValidCommand(123);

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Sale not found");
    }

    /// <summary>
    /// Ensures that an exception is thrown when trying to cancel a sale that is already cancelled.
    /// </summary>
    [Fact(DisplayName = "Given already cancelled sale When cancelling sale Then throws invalid operation exception")]
    public async Task Handle_AlreadyCancelledSale_ThrowsInvalidOperationException()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var sale = CancelSaleCommandTestData.CreateCancelledSale(customerId, branchId);
        var command = CancelSaleCommandTestData.CreateValidCommand(sale.Id);

        _saleRepository.GetByIdAsync(sale.Id, Arg.Any<CancellationToken>())
            .Returns(sale);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("This sale has already been cancelled");
    }
}
