using Neptuo.Templates.Compilation.CodeGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation
{
    /// <summary>
    /// Base implementation of <see cref="INamingService"/>.
    /// </summary>
    public abstract class NamingServiceBase : INamingService
    {
        /// <summary>
        /// Base view name template in form View_{0}.
        /// </summary>
        public const string ViewNameFormat = "View_{0}";

        /// <summary>
        /// Creates naming for file.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <returns>Naming for file.</returns>
        public INaming FromFile(string fileName)
        {
            return GetNaming(fileName, GetNameForFile(fileName));
        }

        /// <summary>
        /// Creates naming for content of template.
        /// </summary>
        /// <param name="viewContent">Template content.</param>
        /// <returns>Naming for content of template.</returns>
        public INaming FromContent(string viewContent)
        {
            return GetNaming(null, GetNameForContent(viewContent));
        }

        /// <summary>
        /// Creates naming using <see cref="DefaultNaming"/> for <paramref name="sourceName"/> and <paramref name="viewName"/>.
        /// </summary>
        /// <param name="sourceName">Template file name.</param>
        /// <param name="viewName">Generated view name.</param>
        /// <returns>Naming using <see cref="DefaultNaming"/> for <paramref name="sourceName"/> and <paramref name="viewName"/>.</returns>
        protected virtual INaming GetNaming(string sourceName, string viewName)
        {
            return new DefaultNaming(
                sourceName,
                CodeDomStructureGenerator.Names.CodeNamespace,
                viewName,
                GetAssemblyForName(viewName)
            );
        }

        /// <summary>
        /// Returns generated view name for template file <paramref name="fileName"/>
        /// </summary>
        /// <param name="fileName">Template file name.</param>
        /// <returns>Generated view name for template file <paramref name="fileName"/></returns>
        protected abstract string GetNameForFile(string fileName);

        /// <summary>
        /// Returns generated view name for template content <paramref name="viewContent"/>
        /// </summary>
        /// <param name="viewContent">Template content.</param>
        /// <returns>Generated view name for template content <paramref name="viewContent"/></returns>
        protected abstract string GetNameForContent(string viewContent);

        /// <summary>
        /// Formats assembly name from <paramref name="viewName"/>.
        /// This implementation simply adds .dll suffix.
        /// </summary>
        /// <param name="viewName">Generated view name.</param>
        /// <returns>Assmebly name for <paramref name="viewName"/>,</returns>
        protected virtual string GetAssemblyForName(string viewName)
        {
            Guard.NotNull(viewName, "viewName");
            return String.Format("{0}.dll", viewName);
        }
    }
}
