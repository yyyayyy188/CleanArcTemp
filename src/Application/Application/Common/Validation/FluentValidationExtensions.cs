using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;
public static class FluentValidationExtensions
{
    public static IServiceCollection RegisterValidatorsInAssembly(this IServiceCollection services, Assembly assembly)
    {
        return services.RegisterValidatorsInAssemblyList(assembly);
    }

    public static IServiceCollection RegisterValidatorsInAssemblyList(this IServiceCollection services, params Assembly[] assemblies)
    {
        var assemblyList = assemblies?.ToList() ?? new List<Assembly>();
        if (!assemblyList.Any())
        {
            return services;
        }

        // register the validators from the respective assemblies
        assemblyList.ForEach(x=>services.AddValidatorsFromAssembly(x));
        
        // register the custom validator factory, to get `IValidator` instances to validate requests
        services.AddSingleton<ICustomValidatorFactory, CustomValidatorFactory>();

        return services;
    }
}