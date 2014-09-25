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
            public CodeDomStructure Structure { get; set; }
            public bool IsDirectObjectResolve { get; private set; }

            public Context(ICodeGeneratorContext codeGeneratorContext, CodeDomGenerator codeGenerator, bool isDirectObjectResolve)
            {
                CodeGeneratorContext = codeGeneratorContext;
                CodeGenerator = codeGenerator;
                IsDirectObjectResolve = isDirectObjectResolve;
            }
        }
    }
}
