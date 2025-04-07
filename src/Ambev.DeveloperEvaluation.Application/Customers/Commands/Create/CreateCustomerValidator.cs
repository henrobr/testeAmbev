using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Customers.Commands.Create;

/// <summary>
/// Validator for the <see cref="CreateCustomerCommand"/> request.
/// Ensures that all necessary fields are correctly filled before processing the customer.
/// </summary>
public class CreateCustomerValidator : AbstractValidator<CreateCustomerCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateCustomerValidator"/> class.
    /// </summary>
    public CreateCustomerValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MinimumLength(2).WithMessage("Name must be at least 2 characters long")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters");
    }
}
