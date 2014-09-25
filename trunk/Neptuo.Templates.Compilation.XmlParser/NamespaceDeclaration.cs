using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation
{
    /// <summary>
    /// Defines namespace registration.
    /// </summary>
    public class NamespaceDeclaration
    {
        /// <summary>
        /// Prefix in XML.
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// Clr namespace.
        /// </summary>
        public string Namespace { get; set; }

        public NamespaceDeclaration(string prefix, string ns)
        {
            Guard.NotNullOrEmpty(ns, "ns");
            Prefix = prefix;
            Namespace = ns;
        }
    }
}
