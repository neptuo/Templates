using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Abstraction of XML node.
    /// </summary>
    public interface IXmlNode
    {
        XmlNodeType NodeType { get; }
        string OuterXml { get; }
    }

    /// <summary>
    /// Supported node types
    /// </summary>
    public enum XmlNodeType
    {
        Element, Comment, Text
    }
}
