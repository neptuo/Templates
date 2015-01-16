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
        public static DefaultCodeDomStructureGenerator SetBaseType<T>(this DefaultCodeDomStructureGenerator generator)
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

        /// <summary>
        /// Sets name of the entry point method.
        /// </summary>
        /// <param name="entryPointName">Name of the entry point method.</param>
        public static DefaultCodeDomStructureGenerator SetEntryPointName(this DefaultCodeDomStructureGenerator generator, string entryPointName)
        {
            Guard.NotNull(generator, "generator");
            Guard.NotNullOrEmpty(entryPointName, "entryPointName");
            generator.EntryPointName = entryPointName;
            return generator;
        }

        /// <summary>
        /// Adds parameter to the entry point method.
        /// </summary>
        /// <param name="parameterType">Type of the parameter.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        public static DefaultCodeDomStructureGenerator AddEntryPointParameter(this DefaultCodeDomStructureGenerator generator, CodeTypeReference parameterType, string parameterName)
        {
            Guard.NotNull(generator, "generator");
            Guard.NotNull(parameterType, "parameterType");
            Guard.NotNullOrEmpty(parameterName, "parameterName");
            generator.EntryPointParameters.Add(new CodeParameterDeclarationExpression(parameterType, parameterName));
            return generator;
        }

        /// <summary>
        /// Adds parameter to the entry point method.
        /// </summary>
        /// <typeparam name="T">Type of the parameter.</typeparam>
        /// <param name="parameterName">Name of the parameter.</param>
        public static DefaultCodeDomStructureGenerator AddEntryPointParameter<T>(this DefaultCodeDomStructureGenerator generator, string parameterName)
        {
            return AddEntryPointParameter(generator, new CodeTypeReference(typeof(T)), parameterName);
        }
    }
}
