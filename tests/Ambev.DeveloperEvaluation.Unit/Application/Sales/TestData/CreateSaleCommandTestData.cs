using Ambev.DeveloperEvaluation.Application.Sales.Commands.Create;
using Ambev.DeveloperEvaluation.Domain.Models.BranchAggregate.Entities;
using Ambev.DeveloperEvaluation.Domain.Models.CustomerAggregate.Entities;
using Ambev.DeveloperEvaluation.Domain.Models.ProductAggregate.Entities;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData;

/// <summary>
/// Provides test data for <see cref="CreateSaleCommandHandler"/> unit tests.
/// </summary>
public static class CreateSaleCommandTestData
{
    /// <summary>
    /// Generates a valid <see cref="CreateSaleCommand"/> with one item.
    /// </summary>
    /// <returns>A valid command instance.</returns>
    public static CreateSaleCommand GenerateValidCommand()
    {
        var customerId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var productId = 123;

        return new CreateSaleCommand(
            customerId,
            branchId,
            new List<SaleItems>
            {
                new(productId, 10)
            }
        );
    }

    /// <summary>
    /// Creates a valid <see cref="Customer"/> instance with the specified ID.
    /// </summary>
    /// <param name="id">The ID to assign to the customer.</param>
    /// <returns>A valid customer entity.</returns>
    public static Customer GetValidCustomer(Guid id)
    {
        var customer = Customer.Create("JOSE TESTE");

        typeof(Customer)
            .GetProperty(nameof(Customer.Id))!
            .SetValue(customer, id);

        return customer;
    }

    /// <summary>
    /// Creates a valid <see cref="Branch"/> instance with the specified ID.
    /// </summary>
    /// <param name="id">The ID to assign to the branch.</param>
    /// <returns>A valid branch entity.</returns>
    public static Branch GetValidBranch(Guid id)
    {
        var branch = Branch.Create("BRANCH - SP");

        typeof(Branch)
            .GetProperty(nameof(Branch.Id))!
            .SetValue(branch, id);

        return branch;
    }

    /// <summary>
    /// Creates a valid <see cref="Product"/> instance with the specified ID.
    /// </summary>
    /// <param name="id">The ID to assign to the product.</param>
    /// <returns>A valid product entity.</returns>
    public static Product GetValidProduct(int id)
    {
        var product = Product.Create("PRODUTO TESTE", 10.00m);

        typeof(Product)
            .GetProperty(nameof(Product.Id))!
            .SetValue(product, id);

        return product;
    }
}
