using Ambev.DeveloperEvaluation.Application.Sales.Commands.Update;
using Ambev.DeveloperEvaluation.Domain.Models.BranchAggregate.Entities;
using Ambev.DeveloperEvaluation.Domain.Models.CustomerAggregate.Entities;
using Ambev.DeveloperEvaluation.Domain.Models.ProductAggregate.Entities;
using Ambev.DeveloperEvaluation.Domain.Models.SaleAggregate.Entities;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData;

/// <summary>
/// Provides test data for <see cref="UpdateSaleCommandHandlerTests"/>.
/// </summary>
public static class UpdateSaleCommandTestData
{
    #region Command

    /// <summary>
    /// Creates a valid <see cref="UpdateSaleCommand"/>.
    /// </summary>
    public static UpdateSaleCommand CreateValidCommand(int saleId, Guid customerId, Guid branchId, int productId)
    {
        return new UpdateSaleCommand(
            saleId,
            customerId,
            branchId,
            new() { new(productId, 2) }
        );
    }

    /// <summary>
    /// Creates an invalid <see cref="UpdateSaleCommand"/> with empty fields.
    /// </summary>
    public static UpdateSaleCommand CreateInvalidCommand()
    {
        return new UpdateSaleCommand(0, Guid.Empty, Guid.Empty, []);
    }

    #endregion

    #region Entities

    /// <summary>
    /// Creates a new <see cref="Sale"/> in the Pending status.
    /// </summary>
    public static Sale CreatePendingSale(Guid customerId, Guid branchId)
    {
        return Sale.Create(customerId, branchId);
    }

    /// <summary>
    /// Creates a <see cref="Sale"/> and marks it as Completed.
    /// </summary>
    public static Sale CreateCompletedSale(Guid customerId, Guid branchId)
    {
        var sale = Sale.Create(customerId, branchId);
        sale.Complete(); 
        return sale;
    }

    /// <summary>
    /// Creates a product with the specified ID and price.
    /// </summary>
    public static Product CreateProduct(int productId, decimal price = 10m)
    {
        var product = Product.Create($"PRODUCT TEST {productId}", 10.00m);

        typeof(Product)
            .GetProperty(nameof(Product.Id))!
            .SetValue(product, productId);

        return product;
    }

    /// <summary>
    /// Creates a customer with the specified ID.
    /// </summary>
    public static Customer CreateCustomer(Guid customerId)
    {
        var customer = Customer.Create("JOSE TEST");

        typeof(Customer)
            .GetProperty(nameof(Customer.Id))!
            .SetValue(customer, customerId);

        return customer;
    }

    /// <summary>
    /// Creates a branch with the specified ID.
    /// </summary>
    public static Branch CreateBranch(Guid branchId)
    {
        var branch = Branch.Create("BRANCH - SP");

        typeof(Branch)
            .GetProperty(nameof(Branch.Id))!
            .SetValue(branch, branchId);

        return branch;
    }

    #endregion
}
