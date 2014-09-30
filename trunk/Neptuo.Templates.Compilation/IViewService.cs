using Neptuo.Templates.Compilation.CodeGenerators;
using Neptuo.Templates.Compilation.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation
{
    /// <summary>
    /// Service that enables template compilation.
    /// According to configuration and implementation, view service can support multiple separate processes and compilations,
    /// these are distinguished by name parameter.
    /// </summary>
    public interface IViewService
    {
        /// <summary>
        /// Compiles <see cref="fileName"/> into view and returns it's instance.
        /// </summary>
        /// <param name="name">Name of required process to run.</param>
        /// <param name="fileName">Template file name.</param>
        /// <param name="context">Context information.</param>
        /// <returns>Instance of compiled template.</returns>
        object Process(string name, string fileName, IViewServiceContext context);

        /// <summary>
        /// Compiles <see cref="viewContent"/> into view and returns it's instance.
        /// </summary>
        /// <param name="name">Name of required process to run.</param>
        /// <param name="viewContent">Template file content.</param>
        /// <param name="context">Context information.</param>
        /// <returns>Instance of compiled template.</returns>
        object ProcessContent(string name, string viewContent, IViewServiceContext context);
    }
}
