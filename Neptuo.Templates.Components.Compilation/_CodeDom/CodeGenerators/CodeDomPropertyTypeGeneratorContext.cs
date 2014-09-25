using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public class CodeDomPropertyTypeGeneratorContext
    {
        public CodeDomGenerator.Context Context { get; private set; }

        public CodeDomPropertyTypeGeneratorContext(CodeDomGenerator.Context context)
        {
            Context = context;
        }
    }
}
