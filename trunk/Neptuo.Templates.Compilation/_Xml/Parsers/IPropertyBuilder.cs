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
    public interface IPropertyBuilder
    {
        /// <summary>
        /// Parses <paramref name="content"/> and creates AST for it.
        /// Value is XML document part.
        /// </summary>
        /// <param name="context">Context inforation.</param>
        /// <param name="codeObject">Targe code object.</param>
        /// <param name="propertyInfo">Target property info.</param>
        /// <param name="content">Source value.</param>
        /// <returns>True if succeed.</returns>
        bool Parse(IContentBuilderContext context, IPropertiesCodeObject codeObject, IPropertyInfo propertyInfo, IEnumerable<IXmlNode> content);

        /// <summary>
        /// Parses <paramref name="content"/> and creates AST for it.
        /// Value is attribute value.
        /// </summary>
        /// <param name="context">Context inforation.</param>
        /// <param name="codeObject">Targe code object.</param>
        /// <param name="propertyInfo">Target property info.</param>
        /// <param name="attributeValue">Source value.</param>
        /// <returns>True if succeed.</returns>
        bool Parse(IContentBuilderContext context, IPropertiesCodeObject codeObject, IPropertyInfo propertyInfo, string attributeValue);
    }
}
