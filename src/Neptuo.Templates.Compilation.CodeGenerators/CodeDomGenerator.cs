using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Code generator using <see cref="System.CodeDom"/> namespace.
    /// </summary>
    public class CodeDomGenerator : ICodeGenerator
    {
        private readonly ICodeDomRegistry registry;

        public CodeDomGenerator(ICodeDomRegistry registry)
        {
            Guard.NotNull(registry, "registry");
            this.registry = registry;
        }

        public bool ProcessTree(ICodeObject codeObject, ICodeGeneratorContext context)
        {
            // 1) Use StructureGenerator to generate base structure.
            ICodeDomStructure structure = registry.WithStructureGenerator().Generate(context);

            // 2) User ObjectGenerator to generate code for codeObject tree.
            CodeExpression expression = registry
                .WithObjectGenerator()
                .Generate(new DefaultCodeDomContext(context, structure, registry), codeObject);

            if (expression == null)
                return false;

            // 3) Append generated expression to entry point method.
            structure.EntryPoint.Statements.Add(expression);

            // 4) Run CodeDom visitors.

            // 5) Generate source code.
            //TODO: Using come registered component.
            WriteOutput(structure.Unit, context.Output);

            return true;
            //throw Guard.Exception.NotImplemented();
        }

        private void WriteOutput(CodeCompileUnit unit, TextWriter writer)
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CodeGeneratorOptions options = new CodeGeneratorOptions
            {
                BracingStyle = "C",
                BlankLinesBetweenMembers = false,
                VerbatimOrder = false
            };

            provider.GenerateCodeFromCompileUnit(unit, writer, options);
        }
    }
}
