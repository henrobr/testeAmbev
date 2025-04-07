using Ambev.DeveloperEvaluation.Domain.Models.BranchAggregate.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

/// <summary>
/// Entity Framework mapping configuration for the <see cref="Branch"/> entity.
/// Defines table structure and column constraints.
/// </summary>
internal class BranchMapping : IEntityTypeConfiguration<Branch>
{
    /// <summary>
    /// Configures the mapping of the <see cref="Branch"/> entity to the database.
    /// </summary>
    /// <param name="builder">EntityTypeBuilder for the Branch entity.</param>
    public void Configure(EntityTypeBuilder<Branch> builder)
    {
        builder.ToTable("Branches");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .HasColumnType("uuid")
            .HasDefaultValueSql("gen_random_uuid()") 
            .IsRequired();

        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(100);
    }
}
