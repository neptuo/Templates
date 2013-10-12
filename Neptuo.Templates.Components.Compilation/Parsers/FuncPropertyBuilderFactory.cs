using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class FuncPropertyBuilderFactory : IPropertyBuilderFactory
    {
        protected Func<IPropertyBuilder> Factory { get; private set; }

        public FuncPropertyBuilderFactory(Func<IPropertyBuilder> factory)
        {
            if (factory == null)
                throw new ArgumentNullException("factory");

            Factory = factory;
        }

        public IPropertyBuilder CreateBuilder(IPropertyInfo propertyInfo)
        {
            return Factory();
        }
    }
}
