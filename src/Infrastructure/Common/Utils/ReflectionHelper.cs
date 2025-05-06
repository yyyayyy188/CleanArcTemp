using System.Reflection;

namespace CleanArchitectureTemp.Infrastructure.Common.Utils;

public static class ReflectionHelper
{
    public static bool IsAssignableToGenericType(Type givenType, Type genericType)
    {
        var givenTypeInfo = givenType.GetTypeInfo();

        if (givenTypeInfo.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
        {
            return true;
        }

        if (givenTypeInfo.GetInterfaces().Any(interfaceType => interfaceType.GetTypeInfo().IsGenericType &&
                                                               interfaceType.GetGenericTypeDefinition() == genericType))
        {
            return true;
        }

        return givenTypeInfo.BaseType != null && IsAssignableToGenericType(givenTypeInfo.BaseType, genericType);
    }
}