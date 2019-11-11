using Microsoft.AspNetCore.JsonPatch;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using Operation = Microsoft.AspNetCore.JsonPatch.Operations.Operation;

namespace Delobytes.AspNetCore.Swagger.SchemaFilters
{
    /// <summary>
    /// Show an example of <see cref="JsonPatchDocument"/> containing all the different patch operations you can do.
    /// </summary>
    /// <seealso cref="ISchemaFilter" />
    public class JsonPatchDocumentSchemaFilter : ISchemaFilter
    {
        /// <summary>
        /// Apply the specified model.
        /// </summary>
        /// <param name="model">Model.</param>
        /// <param name="context">Context.</param>
        public void Apply(OpenApiSchema model, SchemaFilterContext context)
        {
            if (context.ApiModel.Type.GenericTypeArguments.Length > 0 &&
                context.ApiModel.Type.GetGenericTypeDefinition() == typeof(JsonPatchDocument<>))
            {
                Operation[] example = GetExample();
                bool success = OpenApiAnyFactory.TryCreateFor(model, example, out IOpenApiAny openApiAny);

                if (success)
                {
                    model.Default = openApiAny;
                    model.Example = openApiAny;
                }                

                model.ExternalDocs = new OpenApiExternalDocs()
                {
                    Description = "JSON Patch Documentation",
                    Url = new Uri("http://jsonpatch.com/")
                };
            }
        }

        private static Operation[] GetExample()
        {
            return new Operation[]
            {
                new Operation()
                {
                    op = "replace",
                    path = "/property",
                    value = "New Value"
                },
                new Operation()
                {
                    op = "add",
                    path = "/property",
                    value = "New Value"
                },
                new Operation()
                {
                    op = "remove",
                    path = "/property"
                },
                new Operation()
                {
                    op = "copy",
                    @from = "/fromProperty",
                    path = "/toProperty"
                },
                new Operation()
                {
                    op = "move",
                    @from = "/fromProperty",
                    path = "/toProperty"
                },
                new Operation()
                {
                    op = "test",
                    path = "/property",
                    value = "Has Value"
                },
                new Operation()
                {
                    op = "replace",
                    path = "/arrayProperty/0",
                    value = "Replace First Array Item"
                },
                new Operation()
                {
                    op = "replace",
                    path = "/arrayProperty/-",
                    value = "Replace Last Array Item"
                }
            };
        }
    }
}