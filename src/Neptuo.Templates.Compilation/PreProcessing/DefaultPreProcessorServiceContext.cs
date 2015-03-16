using Neptuo.Activators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.PreProcessing
{
    /// <summary>
    /// Implementation of <see cref="IPreProcessorServiceContext"/>.
    /// </summary>
    public class DefaultPreProcessorServiceContext : IPreProcessorServiceContext
    {
        public IDependencyProvider DependencyProvider { get; private set; }

        public DefaultPreProcessorServiceContext(IDependencyProvider dependencyProvider)
        {
            Ensure.NotNull(dependencyProvider, "dependencyProvider");
            DependencyProvider = dependencyProvider;
        }

        public IVisitorContext CreateVisitorContext(IPreProcessorService service)
        {
            Ensure.NotNull(service, "service");
            return new DefaultVisitorContext(DependencyProvider, service);
        }
    }
}
