using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.PreProcessing
{
    public interface IVisitor
    {
        void Visit(IPropertyDescriptor propertyDescriptor, IVisitorContext context);
    }
}
