using Ambev.DeveloperEvaluation.Domain.Models.SaleAggregate.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

/// <summary>
/// Entity Framework mapping configuration for the <see cref="Sale"/> entity.
/// Defines table structure, relationships, and computed columns.
/// </summary>
internal class SaleMapping : IEntityTypeConfiguration<Sale>
{
    /// <summary>
    /// Configures the mapping of the <see cref="Sale"/> entity to the database.
    /// </summary>
    /// <param name="builder">EntityTypeBuilder for Sale entity.</param>
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("Sales");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .UseIdentityAlwaysColumn();

        builder.Property(s => s.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(s => s.CustomerId)
            .IsRequired();

        builder.Property(s => s.BranchId)
            .IsRequired();

        builder.Property(s => s.CreateAt)
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(s => s.UpdateAt)
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Ignore(s => s.TotalAmount);

        builder.HasMany(s => s.Items)
            .WithOne(si => si.Sale)
            .HasForeignKey(si => si.SaleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}