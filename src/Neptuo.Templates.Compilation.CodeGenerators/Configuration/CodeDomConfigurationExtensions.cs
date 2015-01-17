using Neptuo.Collections.Specialized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public static class CodeDomConfigurationExtensions
    {
        public static bool IsDirectObjectResolve(this ICodeDomConfiguration configuration)
        {
            Guard.NotNull(configuration, "configuration");
            return configuration.Get("IsDirectObjectResolve", (bool?)true);
        }
    }
}
