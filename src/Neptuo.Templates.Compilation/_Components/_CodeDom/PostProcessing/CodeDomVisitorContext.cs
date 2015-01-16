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
        public XCodeDomGenerator.Context GeneratorContext { get; private set; }
        public XCodeDomGenerator CodeDomGenerator { get { return GeneratorContext.CodeGenerator; } }
        public CodeDomStructure Structure { get { return GeneratorContext.Structure; } }

        public CodeDomVisitorContext(XCodeDomGenerator.Context generatorContext)
        {
            if (generatorContext == null)
                throw new ArgumentNullException("generatorContext");

            GeneratorContext = generatorContext;
        }
    }
}
