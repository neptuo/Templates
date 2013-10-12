using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class DefaultPropertyBuilderFactory<T> : FuncPropertyBuilderFactory
        where T : IPropertyBuilder, new()
    {
        public DefaultPropertyBuilderFactory()
            : base(() => new T())
        { }
    }
}
