using Neptuo.Web.Framework.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation
{
    public class DefaultParserServiceContext : IParserServiceContext
    {
        public IServiceProvider ServiceProvider { get; private set; }
        public IPropertyDescriptor PropertyDescriptor { get; private set; }

        public DefaultParserServiceContext(IServiceProvider serviceProvider, IPropertyDescriptor propertyDescriptor)
        {
            ServiceProvider = serviceProvider;
            PropertyDescriptor = propertyDescriptor;
        }
    }
}
