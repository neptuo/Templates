using Neptuo.Web.Framework.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.Parsers
{
    public class DefaultParserServiceContext : IParserServiceContext
    {
        public IDependencyProvider DependencyProvider { get; private set; }
        public IPropertyDescriptor PropertyDescriptor { get; private set; }

        public DefaultParserServiceContext(IDependencyProvider dependencyProvider, IPropertyDescriptor propertyDescriptor)
        {
            DependencyProvider = dependencyProvider;
            PropertyDescriptor = propertyDescriptor;
        }
    }
}
