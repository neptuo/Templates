using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public class CodeDomGenerator : ICodeGenerator
    {
        private readonly ICodeDomRegistry generatorRegistry;

        public CodeDomGenerator(ICodeDomRegistry generatorRegistry)
        {
            Guard.NotNull(generatorRegistry, "generatorRegistry");
            this.generatorRegistry = generatorRegistry;
        }

        public bool ProcessTree(ICodeObject codeObject, ICodeGeneratorContext context)
        {
            throw new NotImplementedException();
        }
    }
}
