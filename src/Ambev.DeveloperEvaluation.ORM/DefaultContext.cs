using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Linq.Expressions;
using System.Reflection;

namespace Ambev.DeveloperEvaluation.ORM;

public class DefaultContext : DbContext, IUnitOfWork
{
    public DbSet<User> Users { get; set; }
    public DefaultContext(DbContextOptions<DefaultContext> options) : base(options) { }

    public async Task<bool> CommitAsync(CancellationToken cancellationToken)
    {
        return await base.SaveChangesAsync(cancellationToken) > 0;
    }

    public IQueryable<T> QueryAsNoTracking<T>(params Expression<Func<T, object>>[] includeProperties) where T : class
    {
        var query = base.Set<T>().AsQueryable().AsNoTrackingWithIdentityResolution();

        if (includeProperties is not null)
            query = includeProperties.Aggregate(query, (current, includeProperty) =>
                current.Include(includeProperty));

        return query;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
public class YourDbContextFactory : IDesignTimeDbContextFactory<DefaultContext>
{
    public DefaultContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = new DbContextOptionsBuilder<DefaultContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        builder.UseNpgsql(
               connectionString,
               b => b.MigrationsAssembly(typeof(DefaultContext).Assembly.GetName().Name)
        );

        return new DefaultContext(builder.Options);
    }
}