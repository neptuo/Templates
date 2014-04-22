using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.PreProcessing
{
    public class DefaultPreProcessorService : IPreProcessorService
    {
        private HashSet<IVisitor> visitors = new HashSet<IVisitor>();
        
        public event Action<IPropertyDescriptor, IPreProcessorServiceContext> OnSeachVisitor;

        public void AddVisitor(IVisitor visitor)
        {
            visitors.Add(visitor);
        }

        public void Process(IPropertyDescriptor propertyDescriptor, IPreProcessorServiceContext context)
        {
            if (visitors.Any())
            {
                foreach (IVisitor visitor in visitors)
                    visitor.Visit(propertyDescriptor, new DefaultPreProcessorContext(context.DependencyProvider, this));
            }
            else if (OnSeachVisitor != null)
            {
                OnSeachVisitor(propertyDescriptor, context);
            }
        }
    }
}
