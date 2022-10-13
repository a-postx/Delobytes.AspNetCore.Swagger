using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Delobytes.AspNetCore.Swagger.OperationFilters;

/// <summary>
/// Фильтр требований к безопасности.
/// </summary>
/// <typeparam name="T">Тип</typeparam>
public class SecurityRequirementsOperationFilter<T> : IOperationFilter where T : Attribute
{
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="policySelector">Используется для выбора потилики авторизации из атрибута, напр. (a => a.Policy)</param>
    /// <param name="includeUnauthorizedAndForbiddenResponses">Добавить (по-умолчанию) ответы об ошибках
    /// 401 и 403 в операции атрибутированые Authorize</param>
    /// <param name="securitySchemaName">Имя схемы безопасности. Значение по-умолчанию <c>"oauth2"</c></param>
    /// <param name="unauthorizedResponseDescription">Описание ответа на ошибку 401</param>
    /// <param name="forbiddenResponseDescription">Описание ответа на ошибку 403</param>
    public SecurityRequirementsOperationFilter(Func<IEnumerable<T>, IEnumerable<string?>> policySelector,
        bool includeUnauthorizedAndForbiddenResponses = true,
        string securitySchemaName = "oauth2",
        string unauthorizedResponseDescription = "Не авторизован - Пользователь не предоставил необходимых учётных данных для доступа к ресурсу.",
        string forbiddenResponseDescription = "Запрещено - Пользователь не имеет необходимых прав для доступа к ресурсу.")
    {
        _policySelector = policySelector;
        _includeUnauthorizedAndForbiddenResponses = includeUnauthorizedAndForbiddenResponses;
        _securitySchemaName = securitySchemaName;
        _unauthorizedResponse = new OpenApiResponse { Description = unauthorizedResponseDescription };
        _forbiddenResponse = new OpenApiResponse { Description = forbiddenResponseDescription };
    }

    private readonly Func<IEnumerable<T>, IEnumerable<string?>> _policySelector;
    private readonly bool _includeUnauthorizedAndForbiddenResponses;
    private readonly string _securitySchemaName;

    private const string UnauthorizedStatusCode = "401";
    private const string ForbiddenStatusCode = "403";

    private OpenApiResponse _unauthorizedResponse;
    private OpenApiResponse _forbiddenResponse;

    /// <inheritdoc/>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.ApiDescription.ActionDescriptor as ControllerActionDescriptor != null) // mvc
        {
            if (context.GetControllerAndActionAttributes<AllowAnonymousAttribute>().Any())
            {
                return;
            }

            IEnumerable<T> actionAttributes = context.GetControllerAndActionAttributes<T>();

            if (!actionAttributes.Any())
            {
                return;
            }

            AddDescription(operation, actionAttributes);
        }
        else if (context.ApiDescription.ActionDescriptor as ActionDescriptor != null) //mini api
        {
            IList<object> endpointMetadata = context.ApiDescription.ActionDescriptor.EndpointMetadata;

            IEnumerable<AllowAnonymousAttribute> anonymousMetadataAttributes = endpointMetadata
                                      .Where(a => a as AllowAnonymousAttribute != null)
                                      .Select(a => (AllowAnonymousAttribute)a);

            if (anonymousMetadataAttributes.Any())
            {
                return;
            }

            IEnumerable<T> metadataAttributes = endpointMetadata
                                      .Where(a => a as T != null)
                                      .Select(a => (T)a);

            if (!metadataAttributes.Any())
            {
                return;
            }

            AddDescription(operation, metadataAttributes);
        }
        else
        {
            return;
        }
    }

    private void AddDescription(OpenApiOperation operation, IEnumerable<T> attributes)
    {
        if (_includeUnauthorizedAndForbiddenResponses)
        {
            if (!operation.Responses.ContainsKey(UnauthorizedStatusCode))
            {
                operation.Responses.Add(UnauthorizedStatusCode, _unauthorizedResponse);
            }

            if (!operation.Responses.ContainsKey(ForbiddenStatusCode))
            {
                operation.Responses.Add(ForbiddenStatusCode, _forbiddenResponse);
            }
        }

        IEnumerable<string?> policies = _policySelector(attributes) ?? Enumerable.Empty<string>();

        operation.Security.Add(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = _securitySchemaName }
                    },
                    policies.ToList()
                }
            });
    }
}
