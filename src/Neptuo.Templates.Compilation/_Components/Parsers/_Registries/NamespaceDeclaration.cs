using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class NamespaceDeclaration
    {
        public string Prefix { get; private set; }
        public string NamespaceName { get; private set; }
        public string Assembly { get; private set; }

        public NamespaceDeclaration(string prefix, string namespaceName, string assembly)
        {
            Prefix = prefix;
            NamespaceName = namespaceName;
            Assembly = assembly;
        }
    }
}
