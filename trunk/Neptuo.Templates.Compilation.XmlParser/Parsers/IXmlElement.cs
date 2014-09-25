using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Abstraction of XML element.
    /// </summary>
    public interface IXmlElement : IXmlName, IXmlNode
    {
        /// <summary>
        /// Enumeration of child nodes.
        /// </summary>
        IEnumerable<IXmlNode> ChildNodes { get; }

        /// <summary>
        /// Enumeration of attributes.
        /// </summary>
        IEnumerable<IXmlAttribute> Attributes { get; }

        /// <summary>
        /// <c>true</c> if selfclosed; <c>false</c> otherwise.
        /// </summary>
        bool IsEmpty { get; }
    }
}
