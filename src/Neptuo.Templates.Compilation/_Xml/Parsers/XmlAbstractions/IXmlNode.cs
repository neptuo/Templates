using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Abstraction of any XML node.
    /// </summary>
    public interface IXmlNode
    {
        /// <summary>
        /// Type of node, <see cref="XmlNodeType"/>.
        /// </summary>
        XmlNodeType NodeType { get; }

        /// <summary>
        /// Whole XML including this node as string.
        /// </summary>
        string OuterXml { get; }
    }

    /// <summary>
    /// Supported node types
    /// </summary>
    public enum XmlNodeType
    {
        /// <summary>
        /// XML element.
        /// </summary>
        Element, 
        
        /// <summary>
        /// XML comment.
        /// </summary>
        Comment, 
        
        /// <summary>
        /// Other XML content, such as raw text.
        /// </summary>
        Text
    }
}
