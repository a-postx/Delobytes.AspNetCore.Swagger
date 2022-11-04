using JsonPatchCore;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace Delobytes.AspNetCore.Swagger.SchemaFilters;

/// <summary>
/// Добавляет пример <see cref="OpenApiExample"/>, содержащий доступные операции модификации и ссылку на http://jsonpatch.com.
/// </summary>
/// <seealso cref="ISchemaFilter" />
public class JsonPatchDocumentSchemaFilter : ISchemaFilter
{
    private static readonly OpenApiArray Example = new OpenApiArray()
    {
        new OpenApiObject
            {
                ["op"] = new OpenApiString("replace"),
                ["path"] = new OpenApiString("/property"),
                ["value"] = new OpenApiString("New Value")
            },
            new OpenApiObject
            {
                ["op"] = new OpenApiString("add"),
                ["path"] = new OpenApiString("/property"),
                ["value"] = new OpenApiString("New Value")
            },
            new OpenApiObject
            {
                ["op"] = new OpenApiString("remove"),
                ["path"] = new OpenApiString("/property")
            },
            new OpenApiObject
            {
                ["op"] = new OpenApiString("copy"),
                ["from"] = new OpenApiString("/fromProperty"),
                ["path"] = new OpenApiString("/property")
            },
            new OpenApiObject
            {
                ["op"] = new OpenApiString("move"),
                ["from"] = new OpenApiString("/fromProperty"),
                ["path"] = new OpenApiString("/property")
            },
            new OpenApiObject
            {
                ["op"] = new OpenApiString("test"),
                ["path"] = new OpenApiString("/property"),
                ["value"] = new OpenApiString("Has Value")
            },
            new OpenApiObject
            {
                ["op"] = new OpenApiString("test"),
                ["path"] = new OpenApiString("/property"),
                ["value"] = new OpenApiString("Has Value")
            },
            new OpenApiObject
            {
                ["op"] = new OpenApiString("replace"),
                ["path"] = new OpenApiString("/arrayProperty/0"),
                ["value"] = new OpenApiString("Replace First Array Item")
            },
            new OpenApiObject
            {
                ["op"] = new OpenApiString("replace"),
                ["path"] = new OpenApiString("/arrayProperty/-"),
                ["value"] = new OpenApiString("Replace Last Array Item")
            }
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