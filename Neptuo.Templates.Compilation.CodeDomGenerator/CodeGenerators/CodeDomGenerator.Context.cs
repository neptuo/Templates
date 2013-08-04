using Neptuo.Templates.Compilation.CodeGenerators;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    partial class CodeDomGenerator
    {
        public class Context
        {
            public ICodeGeneratorContext CodeGeneratorContext { get; private set; }
            public CodeDomGenerator CodeGenerator { get; private set; }
            public BaseCodeDomStructure BaseStructure { get; set; }

            public Context(ICodeGeneratorContext codeGeneratorContext, CodeDomGenerator codeGenerator)
            {
                CodeGeneratorContext = codeGeneratorContext;
                CodeGenerator = codeGenerator;
            }
        }
    }
}
