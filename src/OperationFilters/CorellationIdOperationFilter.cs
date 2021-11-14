using System;
using System.Collections.Generic;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Delobytes.AspNetCore.Swagger.OperationFilters;

/// <summary>
/// Добавляет заголовочный <see cref="OpenApiParameter"/> коррелляции для всех операций
/// со значением по-умолчанию в виде случайного гуида.
/// Принимаются параметры: parameterName [= "X-Correlation-ID"]
/// </summary>
/// <seealso cref="IOperationFilter" />
public class CorrelationIdOperationFilter : IOperationFilter
{
    public CorrelationIdOperationFilter(string parameterName = "X-Correlation-ID")
    {
        _parameterName = parameterName;
    }

    private readonly string _parameterName;

    /// <summary>
    /// Применить указанную операцию.
    /// </summary>
    /// <param name="operation">Операция.</param>
    /// <param name="context">Контекст.</param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Parameters is null)
        {
            operation.Parameters = new List<OpenApiParameter>();
        }

        operation.Parameters.Add(
            new OpenApiParameter()
            {
                Description = "Создаёт контекст корелляции: ИД объединяет HTTP-запрос между сервером и клиентом.",
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
