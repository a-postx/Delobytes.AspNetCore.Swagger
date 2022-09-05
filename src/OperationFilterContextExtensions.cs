using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Delobytes.AspNetCore.Swagger;

internal static class OperationFilterContextExtensions
{
    public static IEnumerable<T> GetControllerAndActionAttributes<T>(this OperationFilterContext context) where T : Attribute
    {
        Type? type = context.MethodInfo.DeclaringType;

        if (type == null)
        {
            throw new InvalidOperationException("Unknown type");
        }

        IEnumerable<T> controllerAttributes = type.GetTypeInfo().GetCustomAttributes<T>();
        IEnumerable<T> actionAttributes = context.MethodInfo.GetCustomAttributes<T>();

        List<T> result = new List<T>(controllerAttributes);
        result.AddRange(actionAttributes);

        return result;
    }
}
