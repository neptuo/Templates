using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public interface IXmlElement : IXmlName, IXmlNode
    {
        IEnumerable<IXmlNode> ChildNodes { get; }
        IEnumerable<IXmlAttribute> Attributes { get; }
        bool IsEmpty { get; }
    }
}
