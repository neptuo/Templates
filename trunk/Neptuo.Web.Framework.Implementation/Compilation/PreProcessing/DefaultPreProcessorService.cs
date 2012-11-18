using Neptuo.Web.Framework.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.PreProcessing
{
    public class DefaultPreProcessorService : IPreProcessorService
    {
        private HashSet<IPreProcessor> preProcessors = new HashSet<IPreProcessor>();

        public void AddPreProcessor(IPreProcessor preProcessor)
        {
            preProcessors.Add(preProcessor);
        }

        public void Process(IPropertyDescriptor propertyDescriptor, IPreProcessorServiceContext context)
        {
            foreach (IPreProcessor preProcessor in preProcessors)
                preProcessor.Process(propertyDescriptor, new DefaultPreProcessorContext(context.DependencyProvider, this));
        }
    }
}
