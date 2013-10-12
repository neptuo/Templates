using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class DependencyPropertyBuilderFactory : FuncPropertyBuilderFactory
    {
        public DependencyPropertyBuilderFactory(IDependencyProvider dependencyProvider, Type propertyBuilderType)
            : base(() => (IPropertyBuilder)dependencyProvider.Resolve(propertyBuilderType, null))
        { }
    }

    public class DependencyPropertyBuilderFactory<T> : FuncPropertyBuilderFactory
        where T : IPropertyBuilder
    {
        public DependencyPropertyBuilderFactory(IDependencyProvider dependencyProvider)
            : base(() => dependencyProvider.Resolve<T>())
        { }
    }
}
