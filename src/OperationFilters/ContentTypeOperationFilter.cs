using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace Delobytes.AspNetCore.Swagger.OperationFilters;

/// <summary>
/// Добавляет заголовочный <see cref="OpenApiParameter"/> "Content-Type"
/// с соответствующим типом содержимого к операции.
/// Принимаются параметры: required [= false], contentType [= "application/json"],
/// responseDescription [= "Тип содержимого."].
/// </summary>
/// <seealso cref="IOperationFilter" />
public class ContentTypeOperationFilter : IOperationFilter
{
    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="contentType">Тип содержимого.</param>
    /// <param name="responseDescription">Описание типа содержимого.</param>
    /// <param name="required">Признак обязательности параметра.</param>
    public ContentTypeOperationFilter(bool required = false, string contentType = "application/json",
        string responseDescription = "Тип содержимого.")
    {
        _required = required;
        _contentType = contentType;
        _description = responseDescription;
    }

    private readonly bool _required;
    private readonly string _contentType;
    private readonly string _description;

    /// <inheritdoc/>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Parameters == null)
        {
            operation.Parameters = new List<OpenApiParameter>();
        }

        operation.Parameters.Add(
            new OpenApiParameter()
            {
                Description = _description,
                In = ParameterLocation.Header,
                Name = HeaderNames.ContentType,
                Required = _required,
                Schema = new OpenApiSchema
                {
                    Default = new OpenApiString(_contentType),
                    Type = "string",
                },
            });
    }
}
