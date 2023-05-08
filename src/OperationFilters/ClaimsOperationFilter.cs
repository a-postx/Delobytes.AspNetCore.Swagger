using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Delobytes.AspNetCore.Swagger.OperationFilters;

/// <summary>
/// Добавляет удостоверения из любых требований политики авторизации <see cref="ClaimsAuthorizationRequirement"/>.
/// Принимаются параметры: securitySchemeReferenceId [= "oauth2"]
/// </summary>
/// <seealso cref="IOperationFilter" />
public class ClaimsOperationFilter : IOperationFilter
{
    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="securitySchemeReferenceId">Идентификатор схемы безопасности.</param>
    public ClaimsOperationFilter(string securitySchemeReferenceId = "oauth2")
    {
        _referenceId = securitySchemeReferenceId;
    }

    private readonly string _referenceId;

    /// <inheritdoc/>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        ArgumentNullException.ThrowIfNull(operation, nameof(operation));
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        var filterDescriptors = context.ApiDescription.ActionDescriptor.FilterDescriptors;
        var authorizationRequirements = filterDescriptors.GetPolicyRequirements();
        var claimTypes = authorizationRequirements
            .OfType<ClaimsAuthorizationRequirement>()
            .Select(x => x.ClaimType)
            .ToList();

        if (claimTypes.Any())
        {
            operation.Security = new List<OpenApiSecurityRequirement>()
            {
                new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme() {
                            Reference = new OpenApiReference()
                            {
                                Id = _referenceId,
                                Type = ReferenceType.SecurityScheme,
                            },
                        }
                        , claimTypes 
                    },
                },
            };
        }
    }
}
