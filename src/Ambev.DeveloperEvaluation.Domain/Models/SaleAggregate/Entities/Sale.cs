using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Domain.Models.SaleAggregate.Entities;

/// <summary>
/// Represents a sale transaction in the system.
/// </summary>
public class Sale : EntityInt32
{
    /// <summary>
    /// Gets the current status of the sale.
    /// </summary>
    public SaleStatus Status { get; private set; }

    /// <summary>
    /// Gets the ID of the customer associated with the sale.
    /// </summary>
    public Guid CustomerId { get; private set; }

    /// <summary>
    /// Gets the ID of the branch where the sale occurred.
    /// </summary>
    public Guid BranchId { get; private set; }

    /// <summary>
    /// Gets the date and time when the sale was created (UTC).
    /// </summary>
    public DateTime CreateAt { get; private set; }

    /// <summary>
    /// Gets the date and time when the sale was last updated (UTC).
    /// </summary>
    public DateTime UpdateAt { get; private set; }

    /// <summary>
    /// Gets the total amount of the sale, calculated as the sum of all item totals.
    /// </summary>
    //public decimal TotalAmount => Items.Sum(i => i.TotalPrice);
    public decimal TotalAmount { get; private set; }

    private readonly List<SaleItem> _items = new();

    /// <summary>
    /// Gets the collection of sale items.
    /// </summary>
    public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();

    /// <summary>
    /// Initializes a new instance of the <see cref="Sale"/> class.
    /// Private constructor to enforce controlled creation via factory method.
    /// </summary>
    /// <param name="customerId">The ID of the customer making the purchase.</param>
    /// <param name="branchId">The ID of the branch where the sale is registered.</param>
    private Sale(Guid customerId, Guid branchId)
    {
        Status = SaleStatus.Pending;
        CustomerId = customerId;
        BranchId = branchId;
        CreateAt = DateTime.UtcNow;
        UpdateAt = DateTime.UtcNow;
        CalculateTotalAmount();
    }

    /// <summary>
    /// Factory method for creating a new sale instance.
    /// </summary>
    /// <param name="customerId">The ID of the customer making the purchase.</param>
    /// <param name="branchId">The ID of the branch where the sale is registered.</param>
    /// <returns>A new instance of <see cref="Sale"/>.</returns>
    public static Sale Create(Guid customerId, Guid branchId)
    {
        return new Sale(customerId, branchId);
    }

    /// <summary>
    /// Adds an item to the sale.
    /// </summary>
    /// <param name="productId">The ID of the product being sold.</param>
    /// <param name="quantity">The quantity of the product.</param>
    /// <param name="unitPrice">The unit price of the product.</param>
    public void AddItem(int productId, int quantity, decimal unitPrice)
    {
        var item = SaleItem.Create(productId, quantity, unitPrice);
        _items.Add(item);
        CalculateTotalAmount();
    }

    /// <summary>
    /// Cancels the sale by updating its status to "Cancelled" 
    /// and setting the update timestamp to the current UTC time.
    /// </summary>
    public void Cancel()
    {
        Status = SaleStatus.Cancelled;
        UpdateAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Marks the sale as completed by updating its status to "Completed" 
    /// and setting the update timestamp to the current UTC time.
    /// </summary>
    public void Complete()
    {
        Status = SaleStatus.Completed;
        UpdateAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the branch associated with the sale
    /// and setting the update timestamp to the current UTC time.
    /// </summary>
    /// <param name="branchId">The new branch ID.</param>
    public void UpdateBranch(Guid branchId)
    {
        BranchId = branchId;
        UpdateAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the customer associated with the sale
    /// and setting the update timestamp to the current UTC time.
    /// </summary>
    /// <param name="customerId">The new customer ID.</param>
    public void UpdateCustomer(Guid customerId)
    {
        CustomerId = customerId;
        UpdateAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Removes an item from the sale
    /// and setting the update timestamp to the current UTC time.
    /// </summary>
    /// <param name="productId">The product ID to remove.</param>
    public void RemoveItem(int productId)
    {
        var item = _items.FirstOrDefault(i => i.ProductId == productId);
        if (item is not null)
        {
            _items.Remove(item);
            UpdateAt = DateTime.UtcNow;
            CalculateTotalAmount();
        }
    }

    /// <summary>
    /// Updates the quantity or price of an existing item
    /// and setting the update timestamp to the current UTC time.
    /// </summary>
    /// <param name="productId">The product ID.</param>
    /// <param name="quantity">The new quantity (optional).</param>
    /// <param name="unitPrice">The new unit price (optional).</param>
    public void UpdateItem(int productId, int? quantity = null, decimal? unitPrice = null)
    {
        var item = _items.FirstOrDefault(i => i.ProductId == productId);
        if (item is not null)
        {
            if (quantity.HasValue)
                item.UpdateQuantity(quantity.Value);

            if (unitPrice.HasValue)
                item.UpdatePrice(unitPrice.Value);

            UpdateAt = DateTime.UtcNow;
        }
        CalculateTotalAmount();
    }

    /// <summary>
    /// Removes all items from the sale.
    /// </summary>
    public void ClearItems()
    {
        _items.Clear();
    }

    #region Private Methods

    /// <summary>
    /// Calculates the total amount for the sale by summing the total price of all items.
    /// </summary>
    /// <remarks>
    /// Iterates through all items in the sale and sums their individual total prices
    /// to compute the overall total amount.
    /// </remarks>
    private void CalculateTotalAmount()
    {
        TotalAmount = Items.Sum(i => i.TotalPrice);
    }

    #endregion Private Methods
}
