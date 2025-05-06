using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitectureTemp.Infrastructure.EntityFrameworkCore;

public static class EntityFrameworkCoreInfrastructureInjection 
{
    public static void AddEntityFrameworkCoreInfrastructure<TDbContext>(IServiceCollection services, IConfiguration configuration, int poolSize = 1024)
        where TDbContext : BaseDbContext
    {
        services.AddPooledDbContextFactory<TDbContext>(options => {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
        },poolSize);
        services.AddScoped<BaseDbContextFactory<TDbContext>>();
        services.AddTransient<IDbContextFactory<TDbContext>, BaseDbContextFactory<TDbContext>>();
    }
}