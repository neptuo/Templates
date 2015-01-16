using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public static class _DefaultCodeDomStructureGeneratorExtensions
    {
        /// <summary>
        /// Sets base type of the generated view.
        /// </summary>
        /// <typeparam name="T">Type of base type for generated view.</typeparam>
        public static DefaultCodeDomStructureGenerator AddBaseType<T>(this DefaultCodeDomStructureGenerator generator)
        {
            Guard.NotNull(generator, "generator");
            generator.BaseType = new CodeTypeReference(typeof(T));
            return generator;
        }

        /// <summary>
        /// Adds implemented interface to the generated view.
        /// </summary>
        /// <typeparam name="T">Type of interface to implement by the generated view.</typeparam>
        public static DefaultCodeDomStructureGenerator AddInterface<T>(this DefaultCodeDomStructureGenerator generator)
        {
            Guard.NotNull(generator, "generator");
            generator.ImplementedInterfaces.Add(new CodeTypeReference(typeof(T)));
            return generator;
        }
    }
}
