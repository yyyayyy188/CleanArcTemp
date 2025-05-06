using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitectureTemp.Infrastructure.EntityFrameworkCore.Repositories;

public class EntityFrameworkCoreRepositoryRegistrar
{
    public void RegisterDefaultRepositories<TDbContext>(IServiceCollection services)
    {
        var dbContextType = typeof(TDbContext);
        foreach (var entityType in GetEntityTypes(dbContextType))
        {
            services.AddDefaultRepository(
                entityType,
                typeof(EntityFrameworkCoreBasicRepository<,>).MakeGenericType(dbContextType, entityType));
        }
    }

    private static IEnumerable<Type> GetEntityTypes(Type dbContextType)
    {
        return EntityFrameworkCoreDbContextHelper.GetEntityTypes(dbContextType);
    }
}