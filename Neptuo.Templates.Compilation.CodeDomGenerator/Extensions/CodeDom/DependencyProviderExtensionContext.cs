using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators.Extensions.CodeDom
{
    public class DependencyProviderExtensionContext
    {
        public CodeDomGenerator.Context Context { get; private set; }

        public DependencyProviderExtensionContext(CodeDomGenerator.Context context)
        {
            Context = context;
        }
    }
}
