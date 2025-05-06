using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

public interface ICustomValidatorFactory
{
    IValidator GetValidatorFor(Type type);
}

public class CustomValidatorFactory : ICustomValidatorFactory
{
    private readonly IServiceProvider _serviceProvider;

    public CustomValidatorFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IValidator<T> GetValidator<T>() {
        return _serviceProvider.GetService(typeof(IValidator<T>)) as IValidator<T> 
               ?? throw new InvalidOperationException($"No validator found for type {typeof(T).Name}");
    }

    public IValidator GetValidatorFor(Type type)
    {
        var genericValidatorType = typeof(IValidator<>);
        var specificValidatorType = genericValidatorType.MakeGenericType(type);
        using (var scope = _serviceProvider.CreateScope())
        {
            var validatorInstance = (IValidator)scope.ServiceProvider.GetService(specificValidatorType)!;
            return validatorInstance;
        }
    }
    
}