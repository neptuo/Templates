using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public class CodeDomStructureContext
    {
        public CodeDomGenerator.Context Context { get; private set; }
        public INaming Naming { get; private set; }

        public CodeDomStructureContext(CodeDomGenerator.Context context, INaming naming)
        {
            Context = context;
            Naming = naming;
        }
    }
}
