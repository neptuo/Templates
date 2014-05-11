using Neptuo.Templates.Compilation.CodeGenerators;
using Neptuo.Templates.Compilation.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation
{
    /// <summary>
    /// Service that enables view compilation.
    /// </summary>
    public interface IViewService
    {
        /// <summary>
        /// Compiles <see cref="fileName"/> into view and returns it's instance.
        /// </summary>
        /// <param name="fileName">Template file name.</param>
        /// <param name="context">Context information.</param>
        /// <returns>Instance of compiled template.</returns>
        object Process(string fileName, IViewServiceContext context);

        /// <summary>
        /// Compiles <see cref="viewContent"/> into view and returns it's instance.
        /// </summary>
        /// <param name="viewContent">Template file content.</param>
        /// <param name="context">Context information.</param>
        /// <returns>Instance of compiled template.</returns>
        object ProcessContent(string viewContent, IViewServiceContext context);
    }
}
