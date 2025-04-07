using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.Create;

/// <summary>
/// Validator for the <see cref="CreateSaleCommand"/> request.
/// Ensures that all necessary fields are correctly filled before processing the sale.
/// </summary>
public class CreateSaleValidator : AbstractValidator<CreateSaleCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateSaleValidator"/> class.
    /// </summary>
    public CreateSaleValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("The customer ID must be provided");

        RuleFor(x => x.BranchId)
            .NotEmpty().WithMessage("The branch ID must be provided");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("At least one item must be included in the sale");

        RuleForEach(x => x.Items)
            .SetValidator(new CreateSaleItemValidator());
    }
}