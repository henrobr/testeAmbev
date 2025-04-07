using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.Cancel;

/// <summary>
/// Validator for the <see cref="CancelSaleCommand"/> request.
/// Ensures that all necessary fields are correctly filled before processing the sale.
/// </summary>
public class CancelSaleValidator : AbstractValidator<CancelSaleCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CancelSaleValidator"/> class.
    /// </summary>
    public CancelSaleValidator()
    {
        RuleFor(x => x.SaleId)
            .NotEmpty().WithMessage("The sale ID must be provided");
    }
}