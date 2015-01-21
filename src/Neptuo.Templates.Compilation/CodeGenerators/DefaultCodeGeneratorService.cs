﻿using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Implementation of <see cref="ICodeGeneratorService"/>.
    /// </summary>
    public class DefaultCodeGeneratorService : ICodeGeneratorService
    {
        private readonly Dictionary<string, ICodeGenerator> generators = new Dictionary<string, ICodeGenerator>();

        public ICodeGeneratorService AddGenerator(string name, ICodeGenerator generator)
        {
            Guard.NotNullOrEmpty(name, "name");
            Guard.NotNull(generator, "generator");
            generators[name] = generator;
            return this;
        }

        public bool GeneratedCode(string name, ICodeObject codeObject, ICodeGeneratorServiceContext context)
        {
            Guard.NotNullOrEmpty(name, "name");
            Guard.NotNull(codeObject, "codeObject");
            Guard.NotNull(context, "context");

            ICodeGenerator codeGenerator;
            if (generators.TryGetValue(name, out codeGenerator))
                return codeGenerator.ProcessTree(codeObject, context.CreateGeneratorContext(this));

            throw Guard.Exception.ArgumentOutOfRange("name", "Requested an unregistered code generator named '{0}'.", name);
        }
    }
}