using Neptuo.Activators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.PreProcessing
{
    /// <summary>
    /// Implementation of <see cref="IVisitorContext"/>.
    /// </summary>
    public class DefaultVisitorContext : IVisitorContext
    {
        public IDependencyProvider DependencyProvider { get; private set; }
        public IPreProcessorService PreProcessorService { get; private set; }

        public DefaultVisitorContext(IDependencyProvider dependencyProvider, IPreProcessorService preProcessorService)
        {
            Ensure.NotNull(dependencyProvider, "dependencyProvider");
            Ensure.NotNull(preProcessorService, "preProcessorService");
            DependencyProvider = dependencyProvider;
            PreProcessorService = preProcessorService;
        }
    }
}
