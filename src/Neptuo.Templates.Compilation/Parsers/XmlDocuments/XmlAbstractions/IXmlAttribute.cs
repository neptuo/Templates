using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.XmlDocuments
{
    /// <summary>
    /// Abstraction of XML attribute.
    /// </summary>
    public interface IXmlAttribute : IXmlName
    {
        /// <summary>
        /// Attribute value.
        /// </summary>
        string Value { get; }

        /// <summary>
        /// Xml element that owns this attribute.
        /// </summary>
        IXmlElement OwnerElement { get; }
    }
}
