using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.PreProcessing
{
    /// <summary>
    /// Actual implementation of <see cref="IVisitorContext"/>.
    /// </summary>
    public class DefaultPreProcessorContext : IVisitorContext
    {
        public IDependencyProvider DependencyProvider { get; set; }
        public IPreProcessorService PreProcessorService { get; set; }

        public DefaultPreProcessorContext(IDependencyProvider dependencyProvider, IPreProcessorService preProcessorService)
        {
            DependencyProvider = dependencyProvider;
            PreProcessorService = preProcessorService;
        }
    }
}
