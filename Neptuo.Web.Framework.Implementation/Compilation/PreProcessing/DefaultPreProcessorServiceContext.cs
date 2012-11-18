using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.PreProcessing
{
    public class DefaultPreProcessorServiceContext : IPreProcessorServiceContext
    {
        public IDependencyProvider DependencyProvider { get; set; }

        public DefaultPreProcessorServiceContext(IDependencyProvider dependencyProvider)
        {
            DependencyProvider = dependencyProvider;
        }
    }
}
