using Ambev.DeveloperEvaluation.Domain.Models.ProductAggregate.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

/// <summary>
/// Entity Framework mapping configuration for the <see cref="Product"/> entity.
/// Defines table structure and column constraints.
/// </summary>
internal class ProductMapping : IEntityTypeConfiguration<Product>
{
    /// <summary>
    /// Configures the mapping of the <see cref="Product"/> entity to the database.
    /// </summary>
    /// <param name="builder">EntityTypeBuilder for Product entity.</param>
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .UseIdentityAlwaysColumn();

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");
    }
}
