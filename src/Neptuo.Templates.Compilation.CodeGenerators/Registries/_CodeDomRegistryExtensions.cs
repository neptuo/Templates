using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public static class _CodeDomRegistryExtensions
    {
        #region ICodeDomObjectGenerator

        /// <summary>
        /// Registers <see cref="ICodeDomObjectGenerator"/> to the <paramref name="registry"/>.
        /// One of suggested instances here are <see cref="CodeDomObjectGeneratorRegistry"/>.
        /// </summary>
        /// <param name="registry">Extensible registry for generators.</param>
        /// <param name="generator">Generator for code objects.</param>
        /// <returns><paramref name="registry"/>.</returns>
        public static CodeDomDefaultRegistry AddObjectGenerator(this CodeDomDefaultRegistry registry, ICodeDomObjectGenerator generator)
        {
            Guard.NotNull(registry, "registry");
            return registry.AddRegistry<ICodeDomObjectGenerator>(generator);
        }

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

        /// <summary>
        /// Registers <see cref="ICodeDomObjectGenerator"/> to the <paramref name="registry"/>.
        /// One of suggested instances here are <see cref="CodeDomPropertyGeneratorRegistry"/>.
        /// </summary>
        /// <param name="registry">Extensible registry for generators.</param>
        /// <param name="generator">Generator for code properties.</param>
        /// <returns><paramref name="registry"/>.</returns>
        public static CodeDomDefaultRegistry AddPropertyGenerator(this CodeDomDefaultRegistry registry, ICodeDomPropertyGenerator generator)
        {
            Guard.NotNull(registry, "registry");
            return registry.AddRegistry<ICodeDomPropertyGenerator>(generator);
        }

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

        #region ICodeDomStructureGenerator

        /// <summary>
        /// Registers <see cref="ICodeDomObjectGenerator"/> to the <paramref name="registry"/>.
        /// </summary>
        /// <param name="registry">Extensible registry for generators.</param>
        /// <param name="generator">Generator for base structure.</param>
        /// <returns><paramref name="registry"/>.</returns>
        public static CodeDomDefaultRegistry AddStructureGenerator(this CodeDomDefaultRegistry registry, ICodeDomStructureGenerator generator)
        {
            Guard.NotNull(registry, "registry");
            return registry.AddRegistry<ICodeDomStructureGenerator>(generator);
        }

        public static ICodeDomStructureGenerator WithStructureGenerator(this ICodeDomRegistry registry)
        {
            Guard.NotNull(registry, "registry");
            return registry.With<ICodeDomStructureGenerator>();
        }

        #endregion

        #region ICodeDomDependencyGenerator

        /// <summary>
        /// Registers <see cref="ICodeDomDependencyGenerator"/> to the <paramref name="registry"/>.
        /// </summary>
        /// <param name="registry">Extensible registry for generators.</param>
        /// <param name="generator">Generator for base structure.</param>
        /// <returns><paramref name="registry"/>.</returns>
        public static CodeDomDefaultRegistry AddDependencyGenerator(this CodeDomDefaultRegistry registry, ICodeDomDependencyGenerator generator)
        {
            Guard.NotNull(registry, "registry");
            return registry.AddRegistry<ICodeDomDependencyGenerator>(generator);
        }

        public static ICodeDomDependencyGenerator WithDependencyGenerator(this ICodeDomRegistry registry)
        {
            Guard.NotNull(registry, "registry");
            return registry.With<ICodeDomDependencyGenerator>();
        }

        #endregion

        #region ICodeDomTypeConversionGenerator

        /// <summary>
        /// Registers <see cref="ICodeDomDependencyGenerator"/> to the <paramref name="registry"/>.
        /// </summary>
        /// <param name="registry">Extensible registry for generators.</param>
        /// <param name="generator">Generator dependecies when constructing object instances.</param>
        /// <returns><paramref name="registry"/>.</returns>
        public static CodeDomDefaultRegistry AddTypeConversionGenerator(this CodeDomDefaultRegistry registry, ICodeDomTypeConversionGenerator generator)
        {
            Guard.NotNull(registry, "registry");
            return registry.AddRegistry<ICodeDomTypeConversionGenerator>(generator);
        }

        public static ICodeDomTypeConversionGenerator WithConversionGenerator(this ICodeDomRegistry registry)
        {
            Guard.NotNull(registry, "registry");
            return registry.With<ICodeDomTypeConversionGenerator>();
        }

        #endregion

        #region ICodeDomAttributeGenerator

        /// <summary>
        /// Registers <see cref="ICodeDomAttributeGenerator"/> to the <paramref name="registry"/>.
        /// </summary>
        /// <param name="registry">Extensible registry for generators.</param>
        /// <param name="generator">Generator for processing attributes used on classes or properties.</param>
        /// <returns><paramref name="registry"/>.</returns>
        public static CodeDomDefaultRegistry AddAttributeGenerator(this CodeDomDefaultRegistry registry, ICodeDomAttributeGenerator generator)
        {
            Guard.NotNull(registry, "registry");
            return registry.AddRegistry<ICodeDomAttributeGenerator>(generator);
        }

        public static CodeDomAttributeGeneratorRegistry AddGenerator<TAttribute>(this CodeDomAttributeGeneratorRegistry registry, ICodeDomAttributeGenerator generator)
        {
            Guard.NotNull(registry, "registry");
            return registry.AddGenerator(typeof(TAttribute), generator);
        }

        public static CodeDomAttributeGeneratorRegistry AddDefaultValueGenerator(this CodeDomAttributeGeneratorRegistry registry)
        {
            return AddGenerator<DefaultValueAttribute>(registry, new CodeDomDefaultValueAttributeGenerator());
        }

        public static ICodeDomAttributeGenerator WithAttributeGenerator(this ICodeDomRegistry registry)
        {
            Guard.NotNull(registry, "registry");
            return registry.With<ICodeDomAttributeGenerator>();
        }

        #endregion

        #region ICodeDomVisitor

        /// <summary>
        /// Registers <see cref="ICodeDomVisitor"/> to the <paramref name="registry"/>.
        /// One of suggested instances here are <see cref="CodeDomVisitorRegistry"/>.
        /// </summary>
        /// <param name="registry">Extensible registry for generators.</param>
        /// <param name="visitor">Post processor for generated code.</param>
        /// <returns><paramref name="registry"/>.</returns>
        public static CodeDomDefaultRegistry AddVisitor(this CodeDomDefaultRegistry registry, ICodeDomVisitor visitor)
        {
            Guard.NotNull(registry, "visitor");
            return registry.AddRegistry<ICodeDomVisitor>(visitor);
        }

        public static ICodeDomVisitor WithVisitor(this ICodeDomRegistry registry)
        {
            Guard.NotNull(registry, "registry");
            return registry.With<ICodeDomVisitor>();
        }

        #endregion
    }
}
