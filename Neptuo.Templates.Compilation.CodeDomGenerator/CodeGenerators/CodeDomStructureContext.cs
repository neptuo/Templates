using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public class CodeDomStructureContext
    {
        public CodeDomGenerator.Context GeneratorContext { get; private set; }
        public INaming Naming { get; private set; }

        public CodeDomStructureContext(CodeDomGenerator.Context generatorContext, INaming naming)
        {
            GeneratorContext = generatorContext;
            Naming = naming;
        }
    }
}
