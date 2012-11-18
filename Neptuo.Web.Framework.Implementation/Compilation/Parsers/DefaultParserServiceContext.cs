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
        public ICollection<IErrorInfo> Errors { get; private set; }

        public DefaultParserServiceContext(IDependencyProvider dependencyProvider, IPropertyDescriptor propertyDescriptor, ICollection<IErrorInfo> errors = null)
        {
            DependencyProvider = dependencyProvider;
            PropertyDescriptor = propertyDescriptor;
            Errors = errors ?? new List<IErrorInfo>();
        }
    }
}
