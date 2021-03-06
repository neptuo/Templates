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
    /// Registry for <see cref="ICodeDomObjectGenerator"/> by type of code object.
    /// </summary>
    public class CodeDomObjectGeneratorRegistry : ICodeDomObjectGenerator
    {
        private readonly Dictionary<Type, ICodeDomObjectGenerator> storage = new Dictionary<Type, ICodeDomObjectGenerator>();
        private FuncList<Type, ICodeDomObjectGenerator> onSearchGenerator = new FuncList<Type, ICodeDomObjectGenerator>(o => new NullGenerator());

        /// <summary>
        /// Maps <paramref name="generator"/> to process code objects of type <paramref name="codeObjectType"/>.
        /// </summary>
        /// <param name="codeObjectType">Type of code object to process by <paramref name="generator"/>.</param>
        /// <param name="generator">Generator to process code objects of type <paramref name="codeObjectType"/>.</param>
        public CodeDomObjectGeneratorRegistry AddGenerator(Type codeObjectType, ICodeDomObjectGenerator generator)
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
        public CodeDomObjectGeneratorRegistry AddSearchHandler(Func<Type, ICodeDomObjectGenerator> searchHandler)
        {
            Ensure.NotNull(searchHandler, "searchHandler");
            onSearchGenerator.Add(searchHandler);
            return this;
        }

        public ICodeDomObjectResult Generate(ICodeDomObjectContext context, ICodeObject codeObject)
        {
            Ensure.NotNull(context, "context");
            Ensure.NotNull(codeObject, "codeObject");
            Type codeObjectType = codeObject.GetType();

            ICodeDomObjectGenerator generator;
            if (!storage.TryGetValue(codeObjectType, out generator))
                generator = onSearchGenerator.Execute(codeObjectType);

            return generator.Generate(context, codeObject);
        }

        private class NullGenerator : ICodeDomObjectGenerator
        {
            public ICodeDomObjectResult Generate(ICodeDomObjectContext context, ICodeObject codeObject)
            {
                Type codeObjectType = codeObject.GetType();
                context.AddError("Missing generator for code object of type '{0}'.", codeObjectType.FullName);
                return null;
            }
        }
    }
}
