using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.Complete;

/// <summary>
/// Validator for the <see cref="CompleteSaleCommand"/> request.
/// Ensures that all necessary fields are correctly filled before processing the sale.
/// </summary>
public class CompleteSaleValidator : AbstractValidator<CompleteSaleCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CompleteSaleValidator"/> class.
    /// </summary>
    public CompleteSaleValidator()
    {
        RuleFor(x => x.SaleId)
            .NotEmpty().WithMessage("The sale ID must be provided");
    }
}