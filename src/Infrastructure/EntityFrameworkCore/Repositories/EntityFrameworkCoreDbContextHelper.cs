using System.Reflection;
using CleanArchitectureTemp.Domain.DomainShared.Contracts.Entities;
using CleanArchitectureTemp.Infrastructure.Common.Utils;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureTemp.Infrastructure.EntityFrameworkCore.Repositories;

public static class EntityFrameworkCoreDbContextHelper
{
    /// <summary>
    /// Gets the Entity T.
    /// </summary>
    /// <param name="dbContextType"></param>
    /// <returns></returns>
    public static IEnumerable<Type> GetEntityTypes(Type dbContextType)
    {
        var types =
            from property in dbContextType.GetTypeInfo().GetProperties(BindingFlags.Public | BindingFlags.Instance)
            where
                ReflectionHelper.IsAssignableToGenericType(property.PropertyType, typeof(DbSet<>)) &&
                typeof(IEntity).IsAssignableFrom(property.PropertyType.GenericTypeArguments[0])
            select property.PropertyType.GenericTypeArguments[0];
        return types;
    }
}