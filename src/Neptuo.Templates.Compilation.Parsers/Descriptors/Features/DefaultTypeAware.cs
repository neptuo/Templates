using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Descriptors.Features
{
    public class DefaultTypeAware : ITypeAware
    {
        public Type Type { get; private set; }

        public DefaultTypeAware(Type type)
        {
            Ensure.NotNull(type, "type");
            Type = type;
        }
    }
}
