using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Delobytes.AspNetCore.Swagger.OperationFilters;

/// <summary>
/// Добавляет ответ 401 Неавторизован в ответы для операций, которые атрибутированы
/// политикой <see cref="DenyAnonymousAuthorizationRequirement"/>.
/// </summary>
/// <seealso cref="IOperationFilter" />
public class UnauthorizedResponseOperationFilter : IOperationFilter
{
    private const string UnauthorizedStatusCode = "401";
    private static readonly OpenApiResponse UnauthorizedResponse = new OpenApiResponse()
    {
        Description = "Не Авторизован - Пользователь не предоставил необходимых учётных данных для доступа к ресурсу."
    };

    /// <inheritdoc/>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        ArgumentNullException.ThrowIfNull(operation, nameof(operation));
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        var filterDescriptors = context.ApiDescription.ActionDescriptor.FilterDescriptors;
        var authorizationRequirements = filterDescriptors.GetPolicyRequirements();
        if (!operation.Responses.ContainsKey(UnauthorizedStatusCode) &&
            authorizationRequirements.OfType<DenyAnonymousAuthorizationRequirement>().Any())
        {
            operation.Responses.Add(UnauthorizedStatusCode, UnauthorizedResponse);
        }
    }
}
