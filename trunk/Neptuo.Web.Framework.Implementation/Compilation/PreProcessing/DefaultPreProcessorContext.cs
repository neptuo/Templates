using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.PreProcessing
{
    public class DefaultPreProcessorContext : IPreProcessorContext
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
