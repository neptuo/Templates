using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
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
            CodeExpression expression = registry.WithObjectGenerator().Generate(new DefaultCodeDomContext(context, structure, registry), codeObject);

            throw Guard.Exception.NotImplemented();
        }
    }
}
