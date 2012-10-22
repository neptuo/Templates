using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Neptuo.Web.Framework.Compilation
{
    public partial class CodeDomGenerator : ICodeGenerator
    {
        public GeneratorHelper Helper { get; protected set; }

        public CodeDomGenerator()
        {
            Helper = new GeneratorHelper();
        }

        public bool ProcessTree(ICodeObject rootObject, ICodeGeneratorContext context)
        {
            throw new NotImplementedException();

            WriteOutput(context.Output);
        }

        private void WriteOutput(TextWriter writer)
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CodeGeneratorOptions options = new CodeGeneratorOptions
            {
                BracingStyle = "C",
                BlankLinesBetweenMembers = false,
                VerbatimOrder = false
            };

            provider.GenerateCodeFromCompileUnit(Helper.Unit, writer, options);
        }
    }
}
