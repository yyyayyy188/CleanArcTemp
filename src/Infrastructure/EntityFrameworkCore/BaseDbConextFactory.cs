using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureTemp.Infrastructure.EntityFrameworkCore;


public class BaseDbContextFactory<TDbContext>(IDbContextFactory<TDbContext> contextInFactory)
    : IDbContextFactory<TDbContext> where TDbContext : DbContext
{
    public TDbContext CreateDbContext()
    {
        return contextInFactory.CreateDbContext();
    }


    public async Task<TDbContext> CreateDbContextAsync(CancellationToken cancellationToken = default)
    {
        return await contextInFactory.CreateDbContextAsync(cancellationToken);
    }
}