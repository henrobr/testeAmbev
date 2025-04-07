using Ambev.DeveloperEvaluation.Application.Sales.Commands.Create;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Events.Sale.Created;
using Ambev.DeveloperEvaluation.Domain.Models.BranchAggregate.Entities;
using Ambev.DeveloperEvaluation.Domain.Models.BranchAggregate.Repositories;
using Ambev.DeveloperEvaluation.Domain.Models.CustomerAggregate.Entities;
using Ambev.DeveloperEvaluation.Domain.Models.CustomerAggregate.Repositories;
using Ambev.DeveloperEvaluation.Domain.Models.ProductAggregate.Entities;
using Ambev.DeveloperEvaluation.Domain.Models.ProductAggregate.Repositories;
using Ambev.DeveloperEvaluation.Domain.Models.SaleAggregate.Entities;
using Ambev.DeveloperEvaluation.Domain.Models.SaleAggregate.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData;
using FluentAssertions;
using FluentValidation;
using MediatR;
using NSubstitute;
using System.Reflection;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

/// <summary>
/// Unit tests for <see cref="CreateSaleCommandHandler"/>.
/// </summary>
public class CreateSaleCommandHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly ISaleRepository _saleRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    private readonly CreateSaleCommandHandler _handler;

    /// <summary>
    /// Initializes the test dependencies and the handler under test.
    /// </summary>
    public CreateSaleCommandHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _saleRepository = Substitute.For<ISaleRepository>();
        _branchRepository = Substitute.For<IBranchRepository>();
        _customerRepository = Substitute.For<ICustomerRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _mediator = Substitute.For<IMediator>();

        _handler = new CreateSaleCommandHandler(
            _productRepository,
            _saleRepository,
            _branchRepository,
            _customerRepository,
            _unitOfWork,
            _mediator
        );
    }

    /// <summary>
    /// Verifies that a valid command returns a created sale ID and publishes the correct event.
    /// </summary>
    [Fact(DisplayName = "Given valid command When handling sale Then returns created sale ID")]
    public async Task Handle_ValidCommand_ReturnsSaleId()
    {
        // Arrange
        var command = CreateSaleCommandTestData.GenerateValidCommand();
        var customer = CreateSaleCommandTestData.GetValidCustomer(command.CustomerId);
        var branch = CreateSaleCommandTestData.GetValidBranch(command.BranchId);
        var product = CreateSaleCommandTestData.GetValidProduct(command.Items.First().ProductId);

        _customerRepository
            .GetByIdAsNoTrackingAsync(command.CustomerId, Arg.Any<CancellationToken>())
            .Returns(customer);

        _branchRepository
            .GetByIdAsNoTrackingAsync(command.BranchId, Arg.Any<CancellationToken>())
            .Returns(branch);

        _productRepository
            .GetByIdAsNoTrackingAsync(command.Items.First().ProductId, Arg.Any<CancellationToken>())
            .Returns(product);

        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>()).Returns(true);

        Sale? createdSale = null;
        var generatedSaleId = 123;

        _saleRepository
            .When(x => x.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()))
            .Do(call =>
            {
                createdSale = call.Arg<Sale>();

                var idProperty = typeof(Sale)
                    .GetProperty("Id",
                        BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                if (idProperty != null && idProperty.CanWrite)
                {
                    idProperty.SetValue(createdSale, generatedSaleId);
                }
                else
                {
                    var backingField = typeof(Sale)
                        .GetField("<Id>k__BackingField",
                            BindingFlags.Instance | BindingFlags.NonPublic);

                    backingField?.SetValue(createdSale, generatedSaleId);
                }
            });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        createdSale.Should().NotBeNull();
        result.Should().Be(generatedSaleId);

        await _mediator.Received(1).Publish(
            Arg.Is<SaleCreatedEvent>(e =>
                e.SaleId == generatedSaleId &&
                e.CustomerId == customer.Id &&
                e.BranchId == branch.Id &&
                e.TotalAmount == createdSale!.TotalAmount),
            Arg.Any<CancellationToken>()
        );
    }


    /// <summary>
    /// Verifies that handling an invalid command throws a <see cref="ValidationException"/>.
    /// </summary>
    [Fact(DisplayName = "Given invalid command When handling sale Then throws validation exception")]
    public async Task Handle_InvalidCommand_ThrowsValidationException()
    {
        // Arrange
        var command = new CreateSaleCommand(Guid.Empty, Guid.Empty, []);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }

    /// <summary>
    /// Verifies that handling a command with non-existent customer throws a <see cref="ValidationException"/>.
    /// </summary>
    [Fact(DisplayName = "Given non-existent customer When handling sale Then throws validation exception")]
    public async Task Handle_InvalidCustomer_ThrowsValidationException()
    {
        // Arrange
        var command = CreateSaleCommandTestData.GenerateValidCommand();

        _customerRepository
            .GetByIdAsNoTrackingAsync(command.CustomerId, Arg.Any<CancellationToken>())
            .Returns((Customer?)null);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("*customer does not exist*");
    }

    /// <summary>
    /// Verifies that handling a command with non-existent branch throws a <see cref="ValidationException"/>.
    /// </summary>
    [Fact(DisplayName = "Given non-existent branch When handling sale Then throws validation exception")]
    public async Task Handle_InvalidBranch_ThrowsValidationException()
    {
        // Arrange
        var command = CreateSaleCommandTestData.GenerateValidCommand();

        _customerRepository
            .GetByIdAsNoTrackingAsync(command.CustomerId, Arg.Any<CancellationToken>())
            .Returns(CreateSaleCommandTestData.GetValidCustomer(command.CustomerId));

        _branchRepository
            .GetByIdAsNoTrackingAsync(command.BranchId, Arg.Any<CancellationToken>())
            .Returns((Branch?)null);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("*branch does not exist*");
    }

    /// <summary>
    /// Verifies that handling a command with a non-existent product throws an <see cref="InvalidOperationException"/>.
    /// </summary>
    [Fact(DisplayName = "Given invalid product When handling sale Then throws invalid operation exception")]
    public async Task Handle_InvalidProduct_ThrowsInvalidOperationException()
    {
        // Arrange
        var command = CreateSaleCommandTestData.GenerateValidCommand();

        _customerRepository
            .GetByIdAsNoTrackingAsync(command.CustomerId, Arg.Any<CancellationToken>())
            .Returns(CreateSaleCommandTestData.GetValidCustomer(command.CustomerId));

        _branchRepository
            .GetByIdAsNoTrackingAsync(command.BranchId, Arg.Any<CancellationToken>())
            .Returns(CreateSaleCommandTestData.GetValidBranch(command.BranchId));

        _productRepository
            .GetByIdAsNoTrackingAsync(command.Items.First().ProductId, Arg.Any<CancellationToken>())
            .Returns((Product?)null);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"Product with ID: {command.Items.First().ProductId} was not found");
    }

    /// <summary>
    /// Verifies that if the unit of work fails to commit, a <see cref="ValidationException"/> is thrown.
    /// </summary>
    [Fact(DisplayName = "Given unit of work fails When handling sale Then throws validation exception")]
    public async Task Handle_UnitOfWorkFails_ThrowsValidationException()
    {
        // Arrange
        var command = CreateSaleCommandTestData.GenerateValidCommand();
        var customer = CreateSaleCommandTestData.GetValidCustomer(command.CustomerId);
        var branch = CreateSaleCommandTestData.GetValidBranch(command.BranchId);
        var product = CreateSaleCommandTestData.GetValidProduct(command.Items.First().ProductId);

        _customerRepository
            .GetByIdAsNoTrackingAsync(command.CustomerId, Arg.Any<CancellationToken>())
            .Returns(customer);

        _branchRepository
            .GetByIdAsNoTrackingAsync(command.BranchId, Arg.Any<CancellationToken>())
            .Returns(branch);

        _productRepository
            .GetByIdAsNoTrackingAsync(command.Items.First().ProductId, Arg.Any<CancellationToken>())
            .Returns(product);

        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>()).Returns(false);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("Sale not created");
    }
}
