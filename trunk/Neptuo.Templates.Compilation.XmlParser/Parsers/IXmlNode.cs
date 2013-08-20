using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public interface IXmlNode
    {
        XmlNodeType NodeType { get; }
        string OuterXml { get; }
    }

    public enum XmlNodeType
    {
        Element, Comment, Text
    }
}
