using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Defines builder properties of type.
    /// </summary>
    public interface IContentPropertyBuilder
    {
        /// <summary>
        /// Parses <paramref name="content"/> and creates AST for it.
        /// Value is XML document part.
        /// </summary>
        /// <param name="context">Context inforation.</param>
        /// <param name="content">Source value.</param>
        /// <returns>True if succeeded.</returns>
        IEnumerable<IPropertyDescriptor> TryParse(IContentPropertyBuilderContext context, IEnumerable<IXmlNode> content);
    }
}
