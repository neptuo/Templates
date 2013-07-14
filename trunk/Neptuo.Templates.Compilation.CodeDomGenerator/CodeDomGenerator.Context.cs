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
            public string ClassName { get; private set; }
            public CodeDomGenerator CodeGenerator { get; private set; }

            public CodeCompileUnit Unit { get; set; }
            public CodeNamespace CodeNamespace { get; set; }
            public CodeTypeDeclaration Class { get; set; }
            public CodeMemberMethod CreateViewPageControlsMethod { get; set; }

            public Context(ICodeGeneratorContext codeGeneratorContext, string className, CodeDomGenerator codeGenerator)
            {
                CodeGeneratorContext = codeGeneratorContext;
                ClassName = className;
                CodeGenerator = codeGenerator;
            }
        }
    }
}
