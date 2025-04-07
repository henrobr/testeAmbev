using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Ambev.DeveloperEvaluation.Domain.Models.SaleAggregate.Entities;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

/// <summary>
/// Entity Framework mapping configuration for the <see cref="SaleItem"/> entity.
/// Defines table structure, relationships, and computed columns.
/// </summary>
internal class SaleItemMapping : IEntityTypeConfiguration<SaleItem>
{
    /// <summary>
    /// Configures the mapping of the <see cref="SaleItem"/> entity to the database.
    /// </summary>
    /// <param name="builder">EntityTypeBuilder for SaleItem entity.</param>
    public void Configure(EntityTypeBuilder<SaleItem> builder)
    {
        builder.ToTable("SaleItems");

        builder.HasKey(si => si.Id);

        builder.Property(si => si.Id)
            .UseIdentityAlwaysColumn(); 

        builder.Property(si => si.SaleId)
            .IsRequired();

        builder.Property(si => si.ProductId)
            .IsRequired();

        builder.Property(si => si.Quantity)
            .IsRequired();

        builder.Property(si => si.UnitPrice)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(si => si.Discount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Ignore(s => s.TotalPrice);

        builder.HasOne(si => si.Sale)
            .WithMany(s => s.Items)
            .HasForeignKey(si => si.SaleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}