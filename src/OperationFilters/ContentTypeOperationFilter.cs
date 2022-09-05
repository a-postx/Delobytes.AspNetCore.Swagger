using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace Delobytes.AspNetCore.Swagger.OperationFilters;

/// <summary>
/// Добавляет <see cref="OpenApiParameter"/> ко всем доступным операциям с поддерживаемым типом содержимого.
/// Принимаются параметры: required [= false], contentType [= "application/json"],
/// responseDescription [= "Используется для правильной обработки клиентского запроса (напр. POST)."]
/// </summary>
/// <seealso cref="IOperationFilter" />
public class ContentTypeOperationFilter : IOperationFilter
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="required"></param>
    /// <param name="contentType"></param>
    /// <param name="responseDescription"></param>
    public ContentTypeOperationFilter(bool required = false,
        string contentType = "application/json",
        string responseDescription = "Используется для правильной обработки клиентского запроса (напр. POST).")
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
