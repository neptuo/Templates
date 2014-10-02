using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.ViewActivators
{
    /// <summary>
    /// Service for creating instances of already compiled views.
    /// </summary>
    public interface IViewActivatorService
    {
        /// <summary>
        /// Maps <paramref name="activator"/> to <paramref name="name"/>.
        /// </summary>
        /// <param name="name">Name of activator.</param>
        /// <param name="activator">Activator to register.</param>
        /// <returns>Self (for fluency).</returns>
        IViewActivatorService AddActivator(string name, IViewActivator activator);

        /// <summary>
        /// Creates instance of view represented by <paramref name="viewContent"/>.
        /// </summary>
        /// <param name="name">Name of activator to use.</param>
        /// <param name="viewContent">Content of view.</param>
        /// <param name="context">Context for activation.</param>
        /// <returns>Activated view intance; <c>null</c> of activation was to successfull or compiled view is not accessible.</returns>
        object Activate(string name, string viewContent, IViewActivatorServiceContext context);
    }
}
