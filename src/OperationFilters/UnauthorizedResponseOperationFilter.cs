﻿using System.Linq;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Revolex.AspNetCore.Swagger.OperationFilters
{
    /// <summary>
    /// Add 401 Unauthorized response to a Swagger documentation response when the authorization policy contains
    /// <see cref="DenyAnonymousAuthorizationRequirement"/>.
    /// </summary>
    /// <seealso cref="IOperationFilter" />
    public class UnauthorizedResponseOperationFilter : IOperationFilter
    {
        private const string UnauthorizedStatusCode = "401";
        private static readonly Response UnauthorizedResponse = new Response()
        {
            Description = "Unauthorized - The user has not supplied the necessary credentials to access the resource."
        };

        /// <summary>
        /// Apply the specified operation.
        /// </summary>
        /// <param name="operation">Operation.</param>
        /// <param name="context">Context.</param>
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var filterDescriptors = context.ApiDescription.ActionDescriptor.FilterDescriptors;
            var authorizationRequirements = filterDescriptors.GetPolicyRequirements();
            if (!operation.Responses.ContainsKey(UnauthorizedStatusCode) &&
                authorizationRequirements.OfType<DenyAnonymousAuthorizationRequirement>().Any())
            {
                operation.Responses.Add(UnauthorizedStatusCode, UnauthorizedResponse);
            }
        }
    }
}
