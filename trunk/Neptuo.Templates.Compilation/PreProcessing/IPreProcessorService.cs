using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.PreProcessing
{
    public interface IPreProcessorService
    {
        event Action<IPropertyDescriptor, IPreProcessorServiceContext> OnSeachVisitor;

        void AddVisitor(IVisitor visitor);
        void Process(IPropertyDescriptor propertyDescriptor, IPreProcessorServiceContext context);
    }
}
