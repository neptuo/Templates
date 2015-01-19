using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public static class _CodeDomRegistryExtensions
    {
        #region ICodeDomObjectGenerator

        public static CodeDomObjectGeneratorRegistry AddGenerator<TCodeObject>(this CodeDomObjectGeneratorRegistry registry, ICodeDomObjectGenerator generator)
            where TCodeObject : ICodeObject
        {
            Guard.NotNull(registry, "registry");
            return registry.AddGenerator(typeof(TCodeObject), generator);
        }

        public static ICodeDomObjectGenerator WithObjectGenerator(this ICodeDomRegistry registry)
        {
            Guard.NotNull(registry, "registry");
            return registry.With<ICodeDomObjectGenerator>();
        }

        #endregion

        #region ICodeDomPropertyGenerator

        public static CodeDomPropertyGeneratorRegistry AddGenerator<TCodeObject>(this CodeDomPropertyGeneratorRegistry registry, ICodeDomPropertyGenerator generator)
            where TCodeObject : ICodeProperty
        {
            Guard.NotNull(registry, "registry");
            return registry.AddGenerator(typeof(TCodeObject), generator);
        }

        public static ICodeDomPropertyGenerator WithPropertyGenerator(this ICodeDomRegistry registry)
        {
            Guard.NotNull(registry, "registry");
            return registry.With<ICodeDomPropertyGenerator>();
        }

        #endregion

        public static ICodeDomStructureGenerator WithStructureGenerator(this ICodeDomRegistry registry)
        {
            Guard.NotNull(registry, "registry");
            return registry.With<ICodeDomStructureGenerator>();
        }

        public static ICodeDomDependencyGenerator WithDependencyGenerator(this ICodeDomRegistry registry)
        {
            Guard.NotNull(registry, "registry");
            return registry.With<ICodeDomDependencyGenerator>();
        }

        public static ICodeDomTypeConversionGenerator WithConversionGenerator(this ICodeDomRegistry registry)
        {
            Guard.NotNull(registry, "registry");
            return registry.With<ICodeDomTypeConversionGenerator>();
        }
    }
}
