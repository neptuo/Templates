using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation
{
    public class NamespaceDeclaration
    {
        public string Prefix { get; set; }
        public string Namespace { get; set; }

        public NamespaceDeclaration(string prefix, string ns)
        {
            Prefix = prefix;
            Namespace = ns;
        }
    }
}
