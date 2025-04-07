using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Models.SaleAggregate.Entities;

/// <summary>
/// Represents an individual item in a sale transaction.
/// </summary>
public class SaleItem : EntityInt32
{
    /// <summary>
    /// Gets or sets the ID of the associated sale.
    /// </summary>
    public int SaleId { get; set; }

    /// <summary>
    /// Gets or sets the ID of the product being sold.
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Gets or sets the quantity of the product being sold.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Gets or sets the discount applied to this sale item.
    /// </summary>
    public decimal Discount { get; set; }

    /// <summary>
    /// Gets the total price of the sale item, applying any applicable discount.
    /// </summary>
    //public decimal TotalPrice => UnitPrice * Quantity - Discount;
    public decimal TotalPrice { get; private set; }

    /// <summary>
    /// Gets or sets the associated sale.
    /// </summary>
    public Sale Sale { get; set; }

    /// <summary>
    /// Private constructor to enforce controlled creation via factory method.
    /// </summary>
    /// <param name="productId">The ID of the product.</param>
    /// <param name="quantity">The quantity of the product.</param>
    /// <param name="unitPrice">The unit price of the product.</param>
    private SaleItem(int productId, int quantity, decimal unitPrice)
    {
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Discount = ApplyDiscount();
        CalculateTotalPrice();
    }

    /// <summary>
    /// Factory method to create a new sale item instance.
    /// </summary>
    /// <param name="productId">The ID of the product.</param>
    /// <param name="quantity">The quantity of the product.</param>
    /// <param name="unitPrice">The unit price of the product.</param>
    /// <returns>A new instance of <see cref="SaleItem"/>.</returns>
    public static SaleItem Create(int productId, int quantity, decimal unitPrice)
    {
        return new SaleItem(productId, quantity, unitPrice);
    }

    /// <summary>
    /// Updates the quantity of the sale item and recalculates the discount.
    /// </summary>
    /// <param name="quantity">The new quantity.</param>
    public void UpdateQuantity(int quantity)
    {
        Quantity = quantity;
        Discount = ApplyDiscount();
        CalculateTotalPrice();
    }

    /// <summary>
    /// Updates the unit price of the sale item and recalculates the discount.
    /// </summary>
    /// <param name="unitPrice">The new unit price.</param>
    public void UpdatePrice(decimal unitPrice)
    {
        UnitPrice = unitPrice;
        Discount = ApplyDiscount();
        CalculateTotalPrice();
    }

    /// <summary>
    /// Calculates the total price of an item based on its unit price, quantity, and discount.
    /// </summary>
    /// <remarks>
    /// The total price is computed by multiplying the unit price by the quantity,
    /// and then subtracting any applied discount.
    /// </remarks>
    public void CalculateTotalPrice()
    {
        TotalPrice = UnitPrice * Quantity - Discount;
    }

    #region Private Methods

    /// <summary>
    /// Applies a discount based on the quantity of the product.
    /// </summary>
    /// <returns>The calculated discount amount.</returns>
    /// <exception cref="Exception">Thrown if the quantity exceeds the allowed limit.</exception>
    private decimal ApplyDiscount()
    {
        if (Quantity > 20)
        {
            throw new Exception("You cannot sell more than 20 identical items.");
        }

        if (Quantity > 4 && Quantity < 10)
        {
            return UnitPrice * Quantity * 0.10m;
        }

        if (Quantity >= 10 && Quantity <= 20)
        {
            return UnitPrice * Quantity * 0.20m;
        }

        return 0m;
    }

    #endregion Private Methods
}
