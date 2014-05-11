using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.PreProcessing
{
    /// <summary>
    /// Actual implementation of <see cref="IPreProcessorServiceContext"/>.
    /// </summary>
    public class DefaultPreProcessorServiceContext : IPreProcessorServiceContext
    {
        public IDependencyProvider DependencyProvider { get; set; }

        public DefaultPreProcessorServiceContext(IDependencyProvider dependencyProvider)
        {
            DependencyProvider = dependencyProvider;
        }
    }
}
