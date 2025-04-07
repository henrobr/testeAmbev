using Ambev.DeveloperEvaluation.Application.Sales.Commands.Update;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Events.Sale.Modified;
using Ambev.DeveloperEvaluation.Domain.Models.BranchAggregate.Repositories;
using Ambev.DeveloperEvaluation.Domain.Models.CustomerAggregate.Repositories;
using Ambev.DeveloperEvaluation.Domain.Models.ProductAggregate.Repositories;
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
/// Unit tests for <see cref="UpdateSaleCommandHandler"/>.
/// </summary>
public class UpdateSaleCommandHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    private readonly UpdateSaleCommandHandler _handler;

    public UpdateSaleCommandHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _customerRepository = Substitute.For<ICustomerRepository>();
        _branchRepository = Substitute.For<IBranchRepository>();
        _productRepository = Substitute.For<IProductRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _mediator = Substitute.For<IMediator>();

        _handler = new UpdateSaleCommandHandler(
            _saleRepository,
            _customerRepository,
            _branchRepository,
            _productRepository,
            _unitOfWork,
            _mediator
        );
    }

    /// <summary>
    /// Tests that a valid update command returns true and publishes the <see cref="SaleModifiedEvent"/>.
    /// </summary>
    [Fact(DisplayName = "Given valid update command When handling sale Then returns true")]
    public async Task Handle_ValidCommand_ReturnsTrue()
    {
        // Arrange
        var saleId = 12;
        var customerId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var productId = 101;

        var command = UpdateSaleCommandTestData.CreateValidCommand(saleId, customerId, branchId, productId);
        var sale = UpdateSaleCommandTestData.CreatePendingSale(customerId, branchId);
        var product = UpdateSaleCommandTestData.CreateProduct(productId);
        var customer = UpdateSaleCommandTestData.CreateCustomer(customerId);
        var branch = UpdateSaleCommandTestData.CreateBranch(branchId);

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>()).Returns(sale);
        _customerRepository.GetByIdAsNoTrackingAsync(customerId, Arg.Any<CancellationToken>()).Returns(customer);
        _branchRepository.GetByIdAsNoTrackingAsync(branchId, Arg.Any<CancellationToken>()).Returns(branch);
        _productRepository.GetByIdsAsNoTrackingAsync(Arg.Is<List<int>>(ids => ids.Contains(productId)), Arg.Any<CancellationToken>()).Returns([product]);
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>()).Returns(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();

        await _mediator.Received(1).Publish(
            Arg.Is<SaleModifiedEvent>(e =>
                e.SaleId == sale.Id &&
                e.CustomerId == customerId &&
                e.BranchId == branchId &&
                e.TotalAmount == sale.TotalAmount),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that a command with invalid data throws a <see cref="ValidationException"/>.
    /// </summary>
    [Fact(DisplayName = "Given invalid update command When handling sale Then throws validation exception")]
    public async Task Handle_InvalidCommand_ThrowsValidationException()
    {
        // Arrange
        var command = new UpdateSaleCommand(0, Guid.Empty, Guid.Empty, []);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }

    /// <summary>
    /// Tests that a command referencing a non-existent sale throws a <see cref="ValidationException"/>.
    /// </summary>
    [Fact(DisplayName = "Given non-existent sale When handling sale Then throws validation exception")]
    public async Task Handle_NonExistentSale_ThrowsValidationException()
    {
        // Arrange
        var command = new UpdateSaleCommand(99, Guid.NewGuid(), Guid.NewGuid(), new() { new(1, 1) });

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("Sale not found");
    }

    /// <summary>
    /// Tests that a sale not in pending status cannot be updated.
    /// </summary>
    [Fact(DisplayName = "Given non-pending sale When handling sale Then throws validation exception")]
    public async Task Handle_NonPendingSale_ThrowsValidationException()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var sale = UpdateSaleCommandTestData.CreateCompletedSale(customerId, branchId);

        _saleRepository.GetByIdAsync(12, Arg.Any<CancellationToken>()).Returns(sale);
        var command = UpdateSaleCommandTestData.CreateValidCommand(12, customerId, branchId, 1);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("The sale can only be updated if it has a 'Pending' status");
    }

    /// <summary>
    /// Tests that an update with a non-existent product throws an <see cref="InvalidOperationException"/>.
    /// </summary>
    [Fact(DisplayName = "Given invalid product When handling sale Then throws invalid operation exception")]
    public async Task Handle_InvalidProduct_ThrowsInvalidOperationException()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var productId = 404;
        var sale = UpdateSaleCommandTestData.CreatePendingSale(customerId, branchId);

        _saleRepository.GetByIdAsync(12, Arg.Any<CancellationToken>()).Returns(sale);

        var command = new UpdateSaleCommand(
            12,
            sale.CustomerId,
            sale.BranchId,
            new() { new(productId, 1) });

        _productRepository
            .GetByIdsAsNoTrackingAsync(Arg.Any<List<int>>(), Arg.Any<CancellationToken>())
            .Returns([]);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"Product with ID: {productId} was not found");
    }

    /// <summary>
    /// Tests that when the unit of work fails, the update is not persisted.
    /// </summary>
    [Fact(DisplayName = "Given unit of work fails When handling sale Then throws validation exception")]
    public async Task Handle_UnitOfWorkFails_ThrowsValidationException()
    {
        // Arrange
        var saleId = 12;
        var customerId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var productId = 101;

        var command = new UpdateSaleCommand(
            saleId,
            customerId,
            branchId,
            new() { new(productId, 1) });

        var sale = UpdateSaleCommandTestData.CreateCompletedSale(customerId, branchId);
        var product = UpdateSaleCommandTestData.CreateProduct(productId, 10);
        var customer = UpdateSaleCommandTestData.CreateCustomer(customerId);
        var branch = UpdateSaleCommandTestData.CreateBranch(branchId);

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>()).Returns(sale);
        _customerRepository.GetByIdAsNoTrackingAsync(customerId, Arg.Any<CancellationToken>()).Returns(customer);
        _branchRepository.GetByIdAsNoTrackingAsync(branchId, Arg.Any<CancellationToken>()).Returns(branch);
        _productRepository.GetByIdsAsNoTrackingAsync([productId], Arg.Any<CancellationToken>()).Returns([product]);

        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>()).Returns(false);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("The sale can only be updated if it has a 'Pending' status");
    }
}
