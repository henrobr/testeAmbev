using Ambev.DeveloperEvaluation.Application.Customers.Commands.Create;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Events.Customer.Created;
using Ambev.DeveloperEvaluation.Domain.Models.CustomerAggregate.Entities;
using Ambev.DeveloperEvaluation.Domain.Models.CustomerAggregate.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.Costumers.TestData;
using FluentAssertions;
using MediatR;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Costumers;

/// <summary>
/// Unit tests for <see cref="CreateCustomerCommandHandler"/>.
/// </summary>
public class CreateCustomerCommandHandlerTests
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    private readonly CreateCustomerCommandHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateCustomerCommandHandlerTests"/> class.
    /// Sets up the test dependencies and creates the handler instance.
    /// </summary>
    public CreateCustomerCommandHandlerTests()
    {
        _customerRepository = Substitute.For<ICustomerRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _mediator = Substitute.For<IMediator>();
        _handler = new CreateCustomerCommandHandler(_customerRepository, _unitOfWork, _mediator);
    }

    /// <summary>
    /// Tests that when valid data is provided, the customer is created and its ID is returned.
    /// </summary>
    [Fact(DisplayName = "Given valid data When creating customer Then returns customer ID")]
    public async Task Handle_ValidRequest_ReturnsCustomerId()
    {
        // Arrange
        var command = CreateCustomerCommandHandlerTestData.GenerateValidCommand();
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>()).Returns(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty();
        await _customerRepository.Received(1)
            .CreateAsync(Arg.Is<Customer>(b => b.Name == command.Name.Trim().ToUpper()),
                Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        await _mediator.Received(1)
            .Publish(Arg.Is<CustomerCreatedEvent>(e => e.Name == command.Name.Trim().ToUpper()),
                Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that when invalid data is provided, a validation exception is thrown.
    /// </summary>
    [Fact(DisplayName = "Given invalid data When creating customer Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Arrange
        var command = CreateCustomerCommandHandlerTestData.GenerateInvalidCommand();

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    /// <summary>
    /// Tests that when the commit operation fails, a validation exception is thrown.
    /// </summary>
    [Fact(DisplayName = "Given commit fails When creating customer Then throws validation exception")]
    public async Task Handle_CommitFails_ThrowsValidationException()
    {
        // Arrange
        var command = CreateCustomerCommandHandlerTestData.GenerateValidCommand();
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>()).Returns(false);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>()
            .WithMessage("Customer not created");
    }
}
