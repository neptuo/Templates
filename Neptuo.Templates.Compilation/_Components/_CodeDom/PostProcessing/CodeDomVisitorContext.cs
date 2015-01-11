using Neptuo.Templates.Compilation.CodeGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.PostProcessing
{
    public class CodeDomVisitorContext : ICodeDomVisitorContext
    {
        public CodeDomGenerator.Context GeneratorContext { get; private set; }
        public CodeDomGenerator CodeDomGenerator { get { return GeneratorContext.CodeGenerator; } }
        public CodeDomStructure Structure { get { return GeneratorContext.Structure; } }

        public CodeDomVisitorContext(CodeDomGenerator.Context generatorContext)
        {
            if (generatorContext == null)
                throw new ArgumentNullException("generatorContext");

            GeneratorContext = generatorContext;
        }
    }
}
