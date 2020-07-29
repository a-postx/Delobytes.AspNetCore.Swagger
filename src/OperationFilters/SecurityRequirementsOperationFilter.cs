using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Delobytes.AspNetCore.Swagger.OperationFilters
{
    /// <summary>
    /// Добавляет <see cref="OpenApiSecurityRequirement"/> для всех операций с атрибутом Authorize
    /// (без атрибута AllowAnonymous) и (опционально) ответы 401 Не Авторизован и 403 Запрещено.
    /// Принимаются параметры: includeUnauthorizedAndForbiddenResponses [= true],
    /// securitySchemaName [= "oauth2"],
    /// unauthorizedResponseDescription [= "Не авторизован - Пользователь не предоставил необходимых учётных данных для доступа к ресурсу."]
    /// forbiddenResponseDescription [= "Запрещено - Пользователь не имеет необходимых прав для доступа к ресурсу."]
    /// </summary>
    public class SecurityRequirementsOperationFilter : IOperationFilter
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="includeUnauthorizedAndForbiddenResponses">Добавить (по-умолчанию) ответы об ошибках
        /// 401 и 403 в операции атрибутированые Authorize</param>
        /// <param name="securitySchemaName">Имя схемы безопасности. Значение по-умолчанию <c>"oauth2"</c></param>
        /// <param name="unauthorizedResponseDescription">Описание ответа на ошибку 401</param>
        /// <param name="forbiddenResponseDescription">Описание ответа на ошибку 403</param>
        public SecurityRequirementsOperationFilter(bool includeUnauthorizedAndForbiddenResponses = true,
            string securitySchemaName = "oauth2",
            string unauthorizedResponseDescription = "Не авторизован - Пользователь не предоставил необходимых учётных данных для доступа к ресурсу.",
            string forbiddenResponseDescription = "Запрещено - Пользователь не имеет необходимых прав для доступа к ресурсу.")
        {
            Func<IEnumerable<AuthorizeAttribute>, IEnumerable<string>> policySelector = authAttributes =>
                authAttributes
                    .Where(a => !string.IsNullOrEmpty(a.Policy))
                    .Select(a => a.Policy);

            filter = new SecurityRequirementsOperationFilter<AuthorizeAttribute>(policySelector, includeUnauthorizedAndForbiddenResponses, securitySchemaName, unauthorizedResponseDescription, forbiddenResponseDescription);
        }

        private readonly SecurityRequirementsOperationFilter<AuthorizeAttribute> filter;

        /// <inheritdoc/>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            filter.Apply(operation, context);
        }
    }
}
