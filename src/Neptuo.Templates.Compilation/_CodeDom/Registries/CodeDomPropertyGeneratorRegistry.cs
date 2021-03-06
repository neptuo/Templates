﻿using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Registry for <see cref="ICodeDomPropertyGenerator"/> by type of code object.
    /// </summary>
    public class CodeDomPropertyGeneratorRegistry : ICodeDomPropertyGenerator
    {
        private readonly Dictionary<Type, ICodeDomPropertyGenerator> storage = new Dictionary<Type, ICodeDomPropertyGenerator>();
        private FuncList<Type, ICodeDomPropertyGenerator> onSearchGenerator = new FuncList<Type, ICodeDomPropertyGenerator>(o => new NullGenerator());

        /// <summary>
        /// Maps <paramref name="generator"/> to process code properties of type <paramref name="codeObjectType"/>.
        /// </summary>
        /// <param name="codeObjectType">Type of code property to process by <paramref name="generator"/>.</param>
        /// <param name="generator">Generator to process code properties of type <paramref name="codeObjectType"/>.</param>
        public CodeDomPropertyGeneratorRegistry AddGenerator(Type codeObjectType, ICodeDomPropertyGenerator generator)
        {
            Ensure.NotNull(codeObjectType, "codeObjectType");
            Ensure.NotNull(generator, "generator");
            storage[codeObjectType] = generator;
            return this;
        }

        /// <summary>
        /// Adds <paramref name="searchHandler"/> to be executed when generator was not found.
        /// </summary>
        /// <param name="searchHandler">Generator provider method.</param>
        public CodeDomPropertyGeneratorRegistry AddSearchHandler(Func<Type, ICodeDomPropertyGenerator> searchHandler)
        {
            Ensure.NotNull(searchHandler, "searchHandler");
            onSearchGenerator.Add(searchHandler);
            return this;
        }

        public ICodeDomPropertyResult Generate(ICodeDomPropertyContext context, ICodeProperty codeProperty)
        {
            Ensure.NotNull(context, "context");
            Ensure.NotNull(codeProperty, "codeProperty");
            Type codePropertyType = codeProperty.GetType();

            ICodeDomPropertyGenerator generator;
            if (!storage.TryGetValue(codePropertyType, out generator))
                generator = onSearchGenerator.Execute(codePropertyType);

            return generator.Generate(context, codeProperty);
        }

        private class NullGenerator : ICodeDomPropertyGenerator
        {
            public ICodeDomPropertyResult Generate(ICodeDomPropertyContext context, ICodeProperty codeProperty)
            {
                Type codePropertyType = codeProperty.GetType();
                context.AddError("Missing generator for code property of type '{0}'.", codePropertyType.FullName);
                return null;
            }
        }
    }
}
