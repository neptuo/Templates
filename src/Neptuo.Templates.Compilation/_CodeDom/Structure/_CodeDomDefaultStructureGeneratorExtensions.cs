using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public static class _CodeDomDefaultStructureGeneratorExtensions
    {
        /// <summary>
        /// Sets base type of the generated view.
        /// </summary>
        /// <typeparam name="T">Type of base type for generated view.</typeparam>
        /// <param name="generator">Generator to setup.</param>
        public static CodeDomDefaultStructureGenerator SetBaseType<T>(this CodeDomDefaultStructureGenerator generator)
        {
            Ensure.NotNull(generator, "generator");
            generator.BaseType = new CodeTypeReference(typeof(T));
            return generator;
        }

        /// <summary>
        /// Adds implemented interface to the generated view.
        /// </summary>
        /// <typeparam name="T">Type of interface to implement by the generated view.</typeparam>
        /// <param name="generator">Generator to setup.</param>
        public static CodeDomDefaultStructureGenerator AddInterface<T>(this CodeDomDefaultStructureGenerator generator)
        {
            Ensure.NotNull(generator, "generator");
            generator.ImplementedInterfaces.Add(new CodeTypeReference(typeof(T)));
            return generator;
        }

        /// <summary>
        /// Sets name of the entry point method.
        /// </summary>
        /// <param name="generator">Generator to setup.</param>
        /// <param name="entryPointName">Name of the entry point method.</param>
        public static CodeDomDefaultStructureGenerator SetEntryPointName(this CodeDomDefaultStructureGenerator generator, string entryPointName)
        {
            Ensure.NotNull(generator, "generator");
            Ensure.NotNullOrEmpty(entryPointName, "entryPointName");
            generator.EntryPointName = entryPointName;
            return generator;
        }

        /// <summary>
        /// Adds parameter to the entry point method.
        /// </summary>
        /// <param name="generator">Generator to setup.</param>
        /// <param name="parameterType">Type of the parameter.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        public static CodeDomDefaultStructureGenerator AddEntryPointParameter(this CodeDomDefaultStructureGenerator generator, CodeTypeReference parameterType, string parameterName)
        {
            Ensure.NotNull(generator, "generator");
            Ensure.NotNull(parameterType, "parameterType");
            Ensure.NotNullOrEmpty(parameterName, "parameterName");
            generator.EntryPointParameters.Add(new CodeParameterDeclarationExpression(parameterType, parameterName));
            return generator;
        }

        /// <summary>
        /// Adds parameter to the entry point method.
        /// </summary>
        /// <typeparam name="T">Type of the parameter.</typeparam>
        /// <param name="generator">Generator to setup.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        public static CodeDomDefaultStructureGenerator AddEntryPointParameter<T>(this CodeDomDefaultStructureGenerator generator, string parameterName)
        {
            return AddEntryPointParameter(generator, new CodeTypeReference(typeof(T)), parameterName);
        }
    }
}
