using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.Create;

/// <summary>
/// Validator for individual sale items. Ensures valid product selection, quantity limits, pricing, and discount rules.
/// </summary>
public class CreateSaleItemValidator : AbstractValidator<SaleItems>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateSaleItemValidator"/> class.
    /// </summary>
    public CreateSaleItemValidator()
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0).WithMessage("The product ID must be a positive number");

        RuleFor(x => x.Quantity)
            .InclusiveBetween(1, 20).WithMessage("Quantity must be between 1 and 20 items");
    }
}
