using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Delobytes.AspNetCore.Swagger.OperationFilters;

/// <summary>
/// Добавляет заголовочный <see cref="OpenApiParameter"/> для всех операций
/// со значением по-умолчанию в виде случайного гуида.
/// Принимаются параметры: parameterName [= "Idempotency-Key"]
/// </summary>
/// <seealso cref="IOperationFilter" />
public class IdempotencyKeyOperationFilter : IOperationFilter
{
    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="parameterName">Имя параметра.</param>
    public IdempotencyKeyOperationFilter(string parameterName = "Idempotency-Key")
    {
        _parameterName = parameterName;
    }

    private readonly string _parameterName;

    /// <inheritdoc/>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        IEnumerable<ServiceFilterAttribute> attributes = context.GetControllerAndActionAttributes<ServiceFilterAttribute>();
        IEnumerable<ServiceFilterAttribute> idempotencyAttributes = attributes
            .Where(e => e.ServiceType.Name == "IdempotencyFilterAttribute");

        bool shouldBeIdempotent = idempotencyAttributes.Any();

        if (!shouldBeIdempotent)
        {
            return;
        }

        if (operation.Parameters is null)
        {
            operation.Parameters = new List<OpenApiParameter>();
        }

        operation.Parameters.Add(
            new OpenApiParameter()
            {
                Description = "Идентификатор запроса, используется для контроля идемпотентности.",
                In = ParameterLocation.Header,
                Name = _parameterName,
                Required = true,
                Schema = new OpenApiSchema
                {
                    Default = new OpenApiString(Guid.NewGuid().ToString()),
                    Type = "string",
                },
            });
    }
}
