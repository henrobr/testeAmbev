using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.Update;

/// <summary>
/// Validator for the <see cref="UpdateSaleCommand"/> request.
/// Ensures that all necessary fields are correctly filled before processing the sale.
/// </summary>
public class UpdateSaleValidator : AbstractValidator<UpdateSaleCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateSaleValidator"/> class.
    /// </summary>
    public UpdateSaleValidator(int routeSaleId)
    {
        RuleFor(x => x.SaleId)
            .NotEmpty().WithMessage("The sale ID must be provided")
            .Must(id => id == routeSaleId)
            .WithMessage("Sale ID in the request body must match the sale ID in the route");

        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("The customer ID must be provided");

        RuleFor(x => x.BranchId)
            .NotEmpty().WithMessage("The branch ID must be provided");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("At least one item must be included in the sale");

        When(x => x.Items is not null && x.Items.Any(), () =>
        {
            RuleForEach(x => x.Items)
                .SetValidator(new UpdateSaleItemValidator());
        });
    }
}