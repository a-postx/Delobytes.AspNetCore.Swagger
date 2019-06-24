using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Revolex.AspNetCore.Swagger
{
    /// <summary>
    /// <see cref="SwaggerGenOptions"/>  extension methods.
    /// </summary>
    public static class SwaggerGenOptionsExtensions
    {
        /// <summary>
        /// Include XML comment file if it has the same name as the assembly, 'xml' file extension and exists in
        /// the same directory as the assembly.
        /// </summary>
        /// <param name="options">Swagger options.</param>
        /// <param name="assembly">Assembly.</param>
        /// <returns><c>true</c> if the comment file exists and was added, otherwise <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">options or assembly.</exception>
        public static SwaggerGenOptions IncludeXmlCommentsIfExists(this SwaggerGenOptions options, Assembly assembly)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            string filePath = Path.ChangeExtension(assembly.Location, ".xml");

            if (!IncludeXmlCommentsIfExists(options, filePath) && (assembly.CodeBase != null))
            {
                filePath = Path.ChangeExtension(new Uri(assembly.CodeBase).AbsolutePath, ".xml");
                IncludeXmlCommentsIfExists(options, filePath);
            }

            return options;
        }

        /// <summary>
        /// Include XML comment file if it exists in the specified file path.
        /// </summary>
        /// <param name="options">Swagger options.</param>
        /// <param name="filePath">XML comment file path.</param>
        /// <returns><c>true</c> if the comment file exists and was added, otherwise <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">options or filePath.</exception>
        public static bool IncludeXmlCommentsIfExists(this SwaggerGenOptions options, string filePath)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (filePath == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            if (File.Exists(filePath))
            {
                options.IncludeXmlComments(filePath);

                return true;
            }

            return false;
        }
    }
}
