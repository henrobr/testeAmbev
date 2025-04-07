using Ambev.DeveloperEvaluation.Domain.Models.CustomerAggregate.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

/// <summary>
/// Entity Framework mapping configuration for the <see cref="Customer"/> entity.
/// Defines table structure and column constraints.
/// </summary>
internal class CustomerMapping : IEntityTypeConfiguration<Customer>
{
    /// <summary>
    /// Configures the mapping of the <see cref="Customer"/> entity to the database.
    /// </summary>
    /// <param name="builder">EntityTypeBuilder for Customer entity.</param>
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(c => c.Id);

        builder.Property(b => b.Id)
            .HasColumnType("uuid")
            .HasDefaultValueSql("gen_random_uuid()")
            .IsRequired();

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(150);
    }
}
