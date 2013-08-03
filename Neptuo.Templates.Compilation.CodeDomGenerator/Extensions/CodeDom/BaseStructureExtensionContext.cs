using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators.Extensions.CodeDom
{
    public class BaseStructureExtensionContext
    {
        public CodeDomGenerator.Context Context { get; private set; }

        public BaseStructureExtensionContext(CodeDomGenerator.Context context)
        {
            Context = context;
        }
    }
}
