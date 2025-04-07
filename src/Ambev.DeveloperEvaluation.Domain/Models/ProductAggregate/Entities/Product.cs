using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Models.ProductAggregate.Entities;

/// <summary>
/// Represents a product entity in the system.
/// </summary>
public class Product : EntityInt32
{
    /// <summary>
    /// Gets the name of the product.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets the price of the product.
    /// </summary>
    public decimal Price { get; set; }

    protected Product() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Product"/> class.
    /// Private constructor to enforce controlled creation through the factory method.
    /// </summary>
    /// <param name="name">The name of the product.</param>
    /// <param name="price">The price of the product.</param>
    private Product(string name, decimal price)
    {
        Name = name?.Trim()?.ToUpper();
        Price = price;
    }

    /// <summary>
    /// Factory method for creating a new product instance.
    /// </summary>
    /// <param name="name">The name of the product.</param>
    /// <param name="price">The price of the product.</param>
    /// <returns>A new instance of <see cref="Product"/>.</returns>
    public static Product Create(string name, decimal price)
    {
        return new Product(name, price);
    }

    /// <summary>
    /// Updates the product's name and price.
    /// </summary>
    /// <param name="name">The new name of the product.</param>
    /// <param name="price">The new price of the product.</param>
    public void Update(string name, decimal price)
    {
        Name = name?.Trim()?.ToUpper();
        Price = price;
    }
}
