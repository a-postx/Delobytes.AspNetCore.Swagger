﻿using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace Delobytes.AspNetCore.Swagger.SchemaFilters
{
    /// <summary>
    /// Добавляет пример <see cref="OpenApiExample"/>, содержащий доступные операции модификации
    /// <see cref="JsonPatchDocument"/> и ссылку на http://jsonpatch.com.
    /// </summary>
    /// <seealso cref="ISchemaFilter" />
    public class JsonPatchDocumentSchemaFilter : ISchemaFilter
    {
        private static readonly OpenApiArray Example = new OpenApiArray()
        {
            new OpenApiObject()
            {
                [nameof(Operation.op)] = new OpenApiString("replace"),
                [nameof(Operation.path)] = new OpenApiString("/property"),
                [nameof(Operation.value)] = new OpenApiString("New Value"),
            },
            new OpenApiObject()
            {
                [nameof(Operation.op)] = new OpenApiString("add"),
                [nameof(Operation.path)] = new OpenApiString("/property"),
                [nameof(Operation.value)] = new OpenApiString("New Value"),
            },
            new OpenApiObject()
            {
                [nameof(Operation.op)] = new OpenApiString("remove"),
                [nameof(Operation.path)] = new OpenApiString("/property"),
            },
            new OpenApiObject()
            {
                [nameof(Operation.op)] = new OpenApiString("copy"),
                [nameof(Operation.from)] = new OpenApiString("/fromProperty"),
                [nameof(Operation.path)] = new OpenApiString("/property"),
            },
            new OpenApiObject()
            {
                [nameof(Operation.op)] = new OpenApiString("move"),
                [nameof(Operation.from)] = new OpenApiString("/fromProperty"),
                [nameof(Operation.path)] = new OpenApiString("/property"),
            },
            new OpenApiObject()
            {
                [nameof(Operation.op)] = new OpenApiString("test"),
                [nameof(Operation.path)] = new OpenApiString("/property"),
                [nameof(Operation.value)] = new OpenApiString("Has Value"),
            },
            new OpenApiObject()
            {
                [nameof(Operation.op)] = new OpenApiString("test"),
                [nameof(Operation.path)] = new OpenApiString("/property"),
                [nameof(Operation.value)] = new OpenApiString("Has Value"),
            },
            new OpenApiObject()
            {
                [nameof(Operation.op)] = new OpenApiString("replace"),
                [nameof(Operation.path)] = new OpenApiString("/arrayProperty/0"),
                [nameof(Operation.value)] = new OpenApiString("Replace First Array Item"),
            },
            new OpenApiObject()
            {
                [nameof(Operation.op)] = new OpenApiString("replace"),
                [nameof(Operation.path)] = new OpenApiString("/arrayProperty/-"),
                [nameof(Operation.value)] = new OpenApiString("Replace Last Array Item"),
            },
        };

        private static readonly OpenApiExternalDocs ExternalDocs = new OpenApiExternalDocs()
        {
            Description = "JSON Patch Documentation",
            Url = new Uri("http://jsonpatch.com/", UriKind.Absolute),
        };

        /// <inheritdoc/>
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema is null)
            {
                throw new ArgumentNullException(nameof(schema));
            }

            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Type.GenericTypeArguments.Length > 0 &&
                context.Type.GetGenericTypeDefinition() == typeof(JsonPatchDocument<>))
            {
                schema.Default = Example;
                schema.Example = Example;
                schema.ExternalDocs = ExternalDocs;
            }
        }
    }
}