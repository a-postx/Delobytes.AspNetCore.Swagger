using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Delobytes.AspNetCore.Swagger
{
    /// <summary>
    /// Методы расширения <see cref="SwaggerGenOptions"/>.
    /// </summary>
    public static class SwaggerGenOptionsExtensions
    {
        /// <summary>
        /// Добавить файл с XML-комментариями, если он называется именем библиотеки, имеет расширение .xml
        /// и существует в той же папке, что и библиотека.
        /// </summary>
        /// <param name="options">Настройки свагера.</param>
        /// <param name="assembly">Библиотека.</param>
        /// <returns><c>true</c> если файл комментариев существует и был добавлен, иначе <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">Настройки или путь не предоставлены.</exception>
        public static SwaggerGenOptions IncludeXmlCommentsIfExists(this SwaggerGenOptions options, Assembly assembly)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (assembly is null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            if (!string.IsNullOrEmpty(assembly.Location))
            {
                var filePath = Path.ChangeExtension(assembly.Location, ".xml");
                IncludeXmlCommentsIfExists(options, filePath);
            }

            return options;
        }

        /// <summary>
        /// Включает файл с XML-комментариями, если он существует по указанному пути.
        /// </summary>
        /// <param name="options">Настройки свагера.</param>
        /// <param name="filePath">Путь к файлу с XML-комментариями.</param>
        /// <returns><c>true</c> если файл комментариев существует и был добавлен, иначе <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">Настройки или путь не предоставлены.</exception>
        public static bool IncludeXmlCommentsIfExists(this SwaggerGenOptions options, string filePath)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (filePath is null)
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
