using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.XmlDocuments
{
    /// <summary>
    /// Abstraction of XML named node.
    /// </summary>
    public interface IXmlName
    {
        /// <summary>
        /// Full name, includes prefix (if defined), separator (if prefix defined) and local name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Prefix. Indicates that element belongs to prefixed namespace.
        /// </summary>
        string Prefix { get; }

        /// <summary>
        /// Element name in target namespace.
        /// </summary>
        string LocalName { get; }
    }
}
