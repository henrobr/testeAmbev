using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Models.CustomerAggregate.Entities;

/// <summary>
/// Represents a customer in the system.
/// </summary>
public class Customer : EntityGuid
{
    /// <summary>
    /// Gets the customer's name.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Customer"/> class.
    /// Private constructor to enforce controlled creation through the factory method.
    /// </summary>
    /// <param name="name">The name of the customer.</param>
    private Customer(string name)
    {
        Id = Guid.NewGuid();
        Name = name?.Trim()?.ToUpper();
    }

    /// <summary>
    /// Factory method for creating a new customer instance.
    /// </summary>
    /// <param name="name">The name of the customer.</param>
    /// <returns>A new instance of <see cref="Customer"/>.</returns>
    public static Customer Create(string name)
    {
        return new Customer(name);
    }

    /// <summary>
    /// Updates the customer's name.
    /// </summary>
    /// <param name="name">The new name of the customer.</param>
    public void Update(string name)
    {
        Name = name?.Trim()?.ToUpper();
    }
}