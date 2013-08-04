using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public class DefaultCodeGeneratorService : ICodeGeneratorService
    {
        private Dictionary<string, ICodeGenerator> generators = new Dictionary<string, ICodeGenerator>();

        public void AddGenerator(string name, ICodeGenerator generator)
        {
            generators[name] = generator;
        }

        public bool GeneratedCode(string name, IPropertyDescriptor propertyDescriptor, ICodeGeneratorServiceContext context)
        {
            if (generators.ContainsKey(name))
                return generators[name].ProcessTree(propertyDescriptor, context.CreateGeneratorContext(this));

            return false;
        }
    }
}
