using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Models.BranchAggregate.Entities;

/// <summary>
/// Represents a branch (store or business unit) in the system.
/// </summary>
public class Branch : EntityGuid
{
    /// <summary>
    /// Gets the name of the branch.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Branch"/> class.
    /// Private constructor to enforce controlled creation through the factory method.
    /// </summary>
    /// <param name="name">The name of the branch.</param>
    private Branch(string name)
    {
        Id = Guid.NewGuid();
        Name = name?.Trim()?.ToUpper();
    }

    /// <summary>
    /// Factory method for creating a new branch instance.
    /// </summary>
    /// <param name="name">The name of the branch.</param>
    /// <returns>A new instance of <see cref="Branch"/>.</returns>
    public static Branch Create(string name)
    {
        return new Branch(name);
    }

    /// <summary>
    /// Updates the branch name.
    /// </summary>
    /// <param name="name">The new name of the branch.</param>
    public void Update(string name)
    {
        Name = name?.Trim()?.ToUpper();
    }
}
