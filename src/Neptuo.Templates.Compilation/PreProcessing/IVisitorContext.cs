using Neptuo.Activators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.PreProcessing
{
    /// <summary>
    /// Visitor context.
    /// </summary>
    public interface IVisitorContext
    {
        /// <summary>
        /// Current dependency provider.
        /// </summary>
        IDependencyProvider DependencyProvider { get; }

        /// <summary>
        /// Current pre processing service.
        /// </summary>
        IPreProcessorService PreProcessorService { get; }
    }
}
