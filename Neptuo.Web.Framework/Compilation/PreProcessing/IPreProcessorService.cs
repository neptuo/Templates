using Neptuo.Web.Framework.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.PreProcessing
{
    public interface IPreProcessorService
    {
        void AddPreProcessor(IPreProcessor preProcessor);
        void Process(IPropertyDescriptor propertyDescriptor, IPreProcessorServiceContext context);
    }
}
