using Ambev.DeveloperEvaluation.Application.Sales.Commands.Delete;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Events.Sale.Deleted;
using Ambev.DeveloperEvaluation.Domain.Models.SaleAggregate.Entities;
using Ambev.DeveloperEvaluation.Domain.Models.SaleAggregate.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData;
using FluentAssertions;
using FluentValidation;
using MediatR;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

/// <summary>
/// Unit tests for the <see cref="DeleteSaleCommandHandler"/> class.
/// </summary>
public sealed class DeleteSaleCommandHandlerTests
{
    private readonly ISaleRepository _saleRepository = Substitute.For<ISaleRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private readonly DeleteSaleCommandHandler _handler;

    public DeleteSaleCommandHandlerTests()
    {
        _handler = new DeleteSaleCommandHandler(_saleRepository, _unitOfWork, _mediator);
    }

    /// <summary>
    /// Ensures that a valid sale is deleted successfully.
    /// </summary>
    [Fact]
    public async Task Handle_ValidCommand_DeletesSaleSuccessfully()
    {
        // Arrange
        var sale = DeleteSaleCommandTestData.CreateValidSale();
        var command = new DeleteSaleCommand(sale.Id);

        _saleRepository.GetByIdAsync(sale.Id, Arg.Any<CancellationToken>())
            .Returns(sale);
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>()).Returns(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        _saleRepository.Received().Remove(sale);
        await _mediator.Received().Publish(Arg.Is<SaleDeletedEvent>(e => e.SaleId == sale.Id), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Ensures that a command with an invalid sale ID throws a validation exception.
    /// </summary>
    [Fact]
    public async Task Handle_InvalidCommand_ThrowsValidationException()
    {
        // Arrange
        var command = new DeleteSaleCommand(0);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("*SaleId*");
    }

    /// <summary>
    /// Ensures that deleting a non-existent sale throws a validation exception.
    /// </summary>
    [Fact]
    public async Task Handle_NonExistentSale_ThrowsValidationException()
    {
        // Arrange
        var command = new DeleteSaleCommand(99);

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("Sale not found");
    }

    /// <summary>
    /// Ensures that if the unit of work fails to commit, false is returned.
    /// </summary>
    [Fact]
    public async Task Handle_CommitFails_ReturnsFalse()
    {
        // Arrange
        var sale = DeleteSaleCommandTestData.CreateValidSale();
        var command = new DeleteSaleCommand(sale.Id);

        _saleRepository.GetByIdAsync(sale.Id, Arg.Any<CancellationToken>())
            .Returns(sale);
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>()).Returns(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }
}
