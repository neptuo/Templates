using Neptuo.Templates.Compilation.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.ViewActivators
{
    /// <summary>
    /// Compiled view activator.
    /// </summary>
    public interface IViewActivator
    {
        /// <summary>
        /// Creates instance of view represented by <paramref name="content"/>.
        /// </summary>
        /// <param name="content">Content of view.</param>
        /// <param name="context">Context for activation.</param>
        /// <returns>Activated view intance; <c>null</c> of activation was to successfull or compiled view is not accessible.</returns>
        object Activate(ISourceContent content, IViewActivatorContext context);
    }
}
