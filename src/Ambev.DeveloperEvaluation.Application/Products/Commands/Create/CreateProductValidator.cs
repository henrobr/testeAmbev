using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.Commands.Create;

/// <summary>
/// Validator for the <see cref="CreateProductCommand"/> request.
/// Ensures that all necessary fields are correctly filled before processing the product.
/// </summary>
public class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateProductValidator"/> class.
    /// </summary>
    public CreateProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MinimumLength(2).WithMessage("Name must be at least 2 characters long")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters");

        RuleFor(x => x.Price)
            .GreaterThan(-1).WithMessage("Price must be a positive value");
    }
}
