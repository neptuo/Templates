using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public static class _CodeDomRegistryExtensions
    {
        public static ICodeDomObjectGenerator WithObjectGenerator(this ICodeDomRegistry registry)
        {
            Guard.NotNull(registry, "registry");
            return registry.With<ICodeDomObjectGenerator>();
        }

        public static XICodeDomPropertyGenerator WithPropertyGenerator(this ICodeDomRegistry registry)
        {
            Guard.NotNull(registry, "registry");
            return registry.With<XICodeDomPropertyGenerator>();
        }

        public static ICodeDomStructureGenerator WithStructureGenerator(this ICodeDomRegistry registry)
        {
            Guard.NotNull(registry, "registry");
            return registry.With<ICodeDomStructureGenerator>();
        }
    }
}
