using CleanArchitectureTemp.Domain.DomainShared.Contracts.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CleanArchitectureTemp.Infrastructure.EntityFrameworkCore.Repositories;

public static class EntityFrameworkCoreRepositoryExtensions
{
    public static void AddDefaultRepository(this IServiceCollection services, Type entityType, Type repositoryImplementationType)
    {
        var basicRepository = typeof(IBasicRepository<>).MakeGenericType(entityType);
        if (basicRepository.IsAssignableFrom(repositoryImplementationType))
        {
            RegisterService(services, basicRepository, repositoryImplementationType);
        }
        else
        {
            throw new InvalidOperationException($"The type {repositoryImplementationType} does not implement {basicRepository}");
        }
    }

    private static void RegisterService(IServiceCollection services, Type serviceType, Type implementationType)
    {
        services.TryAddTransient(serviceType, implementationType);
    }
}