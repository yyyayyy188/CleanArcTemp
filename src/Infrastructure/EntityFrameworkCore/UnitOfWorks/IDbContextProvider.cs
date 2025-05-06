namespace CleanArchitectureTemp.Infrastructure.EntityFrameworkCore.UnitOfWorks;

public interface IDbContextProvider<TDbContext>
    where TDbContext : BaseDbContext
{
    /// <summary>
    /// Get the current DbContext instance.
    /// </summary>
    /// <returns></returns>
    Task<TDbContext> GetOrCreateDbContextAsync();

    Task<TDbContext> CreateDbContextAsync();
}