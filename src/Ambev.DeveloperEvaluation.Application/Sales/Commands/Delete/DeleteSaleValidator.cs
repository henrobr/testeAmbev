using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.Delete;

/// <summary>
/// Validator for the <see cref="DeleteSaleCommand"/> request.
/// Ensures that all necessary fields are correctly filled before processing the sale.
/// </summary>
public class DeleteSaleValidator : AbstractValidator<DeleteSaleCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteSaleValidator"/> class.
    /// </summary>
    public DeleteSaleValidator()
    {
        RuleFor(x => x.SaleId)
            .NotEmpty().WithMessage("The sale ID must be provided");
    }
}