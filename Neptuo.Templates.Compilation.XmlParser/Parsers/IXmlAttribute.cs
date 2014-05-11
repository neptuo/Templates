using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Abstraction of XML attribute.
    /// </summary>
    public interface IXmlAttribute : IXmlName
    {
        string Value { get; }
        IXmlElement OwnerElement { get; }
    }
}
