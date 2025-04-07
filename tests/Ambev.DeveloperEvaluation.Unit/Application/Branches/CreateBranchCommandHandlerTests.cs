using Ambev.DeveloperEvaluation.Application.Branches.Commands.Create;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Events.Branch.Created;
using Ambev.DeveloperEvaluation.Domain.Models.BranchAggregate.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.Branches.TestData;
using FluentAssertions;
using MediatR;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Branches;

/// <summary>
/// Unit tests for <see cref="CreateBranchCommandHandler"/>.
/// </summary>
public class CreateBranchCommandHandlerTests
{
    private readonly IBranchRepository _branchRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    private readonly CreateBranchCommandHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateBranchCommandHandlerTests"/> class.
    /// Sets up the test dependencies and handler instance.
    /// </summary>
    public CreateBranchCommandHandlerTests()
    {
        _branchRepository = Substitute.For<IBranchRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _mediator = Substitute.For<IMediator>();
        _handler = new CreateBranchCommandHandler(_branchRepository, _unitOfWork, _mediator);
    }

    /// <summary>
    /// Tests that when valid data is provided, the branch is created and its ID is returned.
    /// </summary>
    [Fact(DisplayName = "Given valid data When creating branch Then returns branch ID")]
    public async Task Handle_ValidRequest_ReturnsBranchId()
    {
        // Arrange
        var command = CreateBranchHandlerTestData.GenerateValidCommand();
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>()).Returns(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty();
        await _branchRepository.Received(1)
            .CreateAsync(Arg.Is<DeveloperEvaluation.Domain.Models.BranchAggregate.Entities.Branch>(b => b.Name == command.Name.Trim().ToUpper()),
                Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        await _mediator.Received(1)
            .Publish(Arg.Is<BranchCreatedEvent>(e => e.Name == command.Name.Trim().ToUpper()),
                Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that when invalid data is provided, a validation exception is thrown.
    /// </summary>
    [Fact(DisplayName = "Given invalid data When creating branch Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Arrange
        var command = CreateBranchHandlerTestData.GenerateInvalidCommand();

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    /// <summary>
    /// Tests that when commit fails, a validation exception is thrown.
    /// </summary>
    [Fact(DisplayName = "Given commit fails When creating branch Then throws validation exception")]
    public async Task Handle_CommitFails_ThrowsValidationException()
    {
        // Arrange
        var command = CreateBranchHandlerTestData.GenerateValidCommand();
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>()).Returns(false);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>()
            .WithMessage("Branch not created");
    }
}
