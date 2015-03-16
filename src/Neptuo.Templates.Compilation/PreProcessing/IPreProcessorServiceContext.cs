using Neptuo.Activators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.PreProcessing
{
    /// <summary>
    /// Pre-processing service context.
    /// </summary>
    public interface IPreProcessorServiceContext
    {
        /// <summary>
        /// Current dependency provider.
        /// </summary>
        IDependencyProvider DependencyProvider { get; }

        /// <summary>
        /// Factory method for creating <see cref="IVisitorContext"/>.
        /// </summary>
        /// <param name="service">Current pre-processing service.</param>
        /// <returns><see cref="IVisitorContext"/>.</returns>
        IVisitorContext CreateVisitorContext(IPreProcessorService service);
    }
}
