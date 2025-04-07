using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.Commands.Create;

/// <summary>
/// Represents a command to create a new branch.
/// </summary>
/// <param name="Name">The name of the branch to be created.</param>
public sealed record CreateBranchCommand(string Name) : IRequest<Guid>;