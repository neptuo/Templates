using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.PreProcessing
{
    public abstract class BaseVisitor : IVisitor
    {
        protected IVisitorContext Context { get; private set; }

        public void Visit(IPropertyDescriptor propertyDescriptor, IVisitorContext context)
        {
            Context = context;
            Visit(propertyDescriptor);
        }

        public void Visit(ICodeObject codeObject, IVisitorContext context)
        {
            Context = context;
            Visit(codeObject);
        }

        protected void Visit(IEnumerable<IPropertyDescriptor> properties)
        {
            foreach (IPropertyDescriptor propertyDescriptor in properties)
                Visit(propertyDescriptor);
        }

        protected void Visit(IPropertyDescriptor propertyDescriptor)
        {
            if (propertyDescriptor is ListAddPropertyDescriptor)
                Visit((ListAddPropertyDescriptor)propertyDescriptor);
            else if (propertyDescriptor is SetPropertyDescriptor)
                Visit((SetPropertyDescriptor)propertyDescriptor);
            else if (propertyDescriptor is MethodInvokePropertyDescriptor)
                Visit((MethodInvokePropertyDescriptor)propertyDescriptor);
        }

        //TODO: Implement codeObject visitor
        protected abstract void Visit(ICodeObject codeObject);

        protected virtual void Visit(ListAddPropertyDescriptor propertyDescriptor)
        {
            foreach (ICodeObject codeObject in propertyDescriptor.Values)
                Visit(codeObject);
        }

        protected virtual void Visit(SetPropertyDescriptor propertyDescriptor)
        {
            Visit(propertyDescriptor.Value);
        }

        protected virtual void Visit(MethodInvokePropertyDescriptor propertyDescriptor)
        {
            foreach (ICodeObject codeObject in propertyDescriptor.Parameters)
                Visit(codeObject);
        }
    }
}
