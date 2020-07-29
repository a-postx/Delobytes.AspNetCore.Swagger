using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Delobytes.AspNetCore.Swagger.OperationFilters
{
    /// <summary>
    /// Добавляет ответ 403 Запрещено в ответы для операций, которые атрибутированы
    /// политикой <see cref="ClaimsAuthorizationRequirement"/>, <see cref="NameAuthorizationRequirement"/>,
    /// <see cref="OperationAuthorizationRequirement"/>, <see cref="RolesAuthorizationRequirement"/> или
    /// <see cref="AssertionRequirement"/>.
    /// </summary>
    /// <seealso cref="IOperationFilter" />
    public class ForbiddenResponseOperationFilter : IOperationFilter
    {
        private const string ForbiddenStatusCode = "403";
        private static readonly OpenApiResponse ForbiddenResponse = new OpenApiResponse()
        {
            Description = "Запрещено - Пользователь не имеет необходимых прав для доступа к ресурсу."
        };

        /// <inheritdoc/>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation is null)
            {
                throw new ArgumentNullException(nameof(operation));
            }

            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var filterDescriptors = context.ApiDescription.ActionDescriptor.FilterDescriptors;
            IList<IAuthorizationRequirement> authorizationRequirements = filterDescriptors.GetPolicyRequirements();
            if (!operation.Responses.ContainsKey(ForbiddenStatusCode) &&
                HasAuthorizationRequirement(authorizationRequirements))
            {
                operation.Responses.Add(ForbiddenStatusCode, ForbiddenResponse);
            }
        }

        private static bool HasAuthorizationRequirement(IEnumerable<IAuthorizationRequirement> authorizationRequirements)
        {
            foreach (IAuthorizationRequirement authorizationRequirement in authorizationRequirements)
            {
                if (authorizationRequirement is ClaimsAuthorizationRequirement ||
                    authorizationRequirement is NameAuthorizationRequirement ||
                    authorizationRequirement is OperationAuthorizationRequirement ||
                    authorizationRequirement is RolesAuthorizationRequirement ||
                    authorizationRequirement is AssertionRequirement)
                {
                    return true;
                }
            }

            return false;
        }
    }
}