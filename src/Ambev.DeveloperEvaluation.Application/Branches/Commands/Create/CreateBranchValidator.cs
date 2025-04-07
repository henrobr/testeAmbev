using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Branches.Commands.Create;

/// <summary>
/// Validator for the <see cref="CreateBranchCommand"/> request.
/// Ensures that all necessary fields are correctly filled before processing the branch.
/// </summary>
public class CreateBranchValidator : AbstractValidator<CreateBranchCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateBranchValidator"/> class.
    /// </summary>
    public CreateBranchValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MinimumLength(2).WithMessage("Name must be at least 2 characters long")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters");
    }
}
