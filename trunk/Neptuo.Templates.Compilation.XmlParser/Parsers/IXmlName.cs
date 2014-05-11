using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Abstraction of XML named node.
    /// </summary>
    public interface IXmlName
    {
        string Name { get; }

        string Prefix { get; }
        string LocalName { get; }
    }
}
