using Ambev.DeveloperEvaluation.Application.Products.Commands.Create;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Events.Product.Created;
using Ambev.DeveloperEvaluation.Domain.Models.ProductAggregate.Entities;
using Ambev.DeveloperEvaluation.Domain.Models.ProductAggregate.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.Products.TestData;
using FluentAssertions;
using MediatR;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products;

/// <summary>
/// Contains unit tests for <see cref="CreateProductCommandHandler"/> to verify behavior when creating products.
/// </summary>
public class CreateProductCommandHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    private readonly CreateProductCommandHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateProductCommandHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public CreateProductCommandHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _mediator = Substitute.For<IMediator>();
        _handler = new CreateProductCommandHandler(_productRepository, _unitOfWork, _mediator);
    }

    /// <summary>
    /// Tests that a valid command results in a product being created and an ID greater than zero being returned.
    /// Also ensures the correct repository and mediator methods are called.
    /// </summary>
    [Fact(DisplayName = "Given valid data When creating product Then returns product ID")]
    public async Task Handle_ValidRequest_ReturnsProductId()
    {
        // Arrange
        var command = CreateProductCommandHandlerTestData.GenerateValidCommand();

        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>()).Returns(true);

        _productRepository
            .When(x => x.CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>()))
            .Do(call =>
            {
                var product = call.Arg<Product>();

                var idField = typeof(Product)
                    .GetProperty("Id",
                        System.Reflection.BindingFlags.Instance
                        | System.Reflection.BindingFlags.NonPublic
                        | System.Reflection.BindingFlags.Public);

                if (idField != null && idField.CanWrite)
                {
                    idField.SetValue(product, 1);
                }
                else
                {
                    var backingField = typeof(Product)
                        .GetField("<Id>k__BackingField",
                            System.Reflection.BindingFlags.Instance
                            | System.Reflection.BindingFlags.NonPublic);
                    backingField?.SetValue(product, 1);
                }
            });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeGreaterThan(0);
        await _productRepository.Received(1).CreateAsync(
            Arg.Is<Product>(p => p.Name == command.Name.Trim().ToUpper()
                && p.Price == command.Price),
            Arg.Any<CancellationToken>()
        );
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        await _mediator.Received(1).Publish(
            Arg.Is<ProductCreatedEvent>(e => e.Name == command.Name.Trim().ToUpper()
                && e.Price == command.Price),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// Tests that an invalid command triggers a <see cref="FluentValidation.ValidationException"/>.
    /// </summary>
    [Fact(DisplayName = "Given invalid data When creating product Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Arrange
        var command = CreateProductCommandHandlerTestData.GenerateInvalidCommand();

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    /// <summary>
    /// Tests that if commit fails, a <see cref="FluentValidation.ValidationException"/> is thrown with a specific message.
    /// </summary>
    [Fact(DisplayName = "Given commit fails When creating product Then throws validation exception")]
    public async Task Handle_CommitFails_ThrowsValidationException()
    {
        // Arrange
        var command = CreateProductCommandHandlerTestData.GenerateValidCommand();

        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>()).Returns(false);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>()
            .WithMessage("Product not created");
    }
}
