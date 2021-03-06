﻿using Neptuo.Text.Positions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Common extenions on <see cref="IXmlElement"/>, <see cref="IXmlAttribute"/> etc.
    /// </summary>
    public static class XmlExtensions
    {
        /// <summary>
        /// Returns <c>true</c> if attribute <paramref name="attributeName"/> is defined on <paramref name="element"/>.
        /// </summary>
        /// <param name="element">Source xml element.</param>
        /// <param name="attributeName">Attribute name.</param>
        /// <returns><c>true</c> if <paramref name="attributeName"/> is defined on <paramref name="element"/>; <c>false</c> otherwise.</returns>
        public static bool HasAttribute(this IXmlElement element, string attributeName)
        {
            Ensure.NotNull(element, "element");
            Ensure.NotNullOrEmpty(attributeName, "attributeName");
            return element.Attributes.Any(a => a.Name == attributeName);
        }

        /// <summary>
        /// Returns value of attribute <paramref name="attributeName"/> defined on <paramref name="element"/>.
        /// </summary>
        /// <param name="element">Source xml element.</param>
        /// <param name="attributeName">Attribute name.</param>
        /// <returns>Value of attribute; <c>null</c> if attribute is not found.</returns>
        public static string GetAttributeValue(this IXmlElement element, string attributeName)
        {
            Ensure.NotNull(element, "element");
            Ensure.NotNullOrEmpty(attributeName, "attributeName");
            IXmlAttribute attribute = element.Attributes.FirstOrDefault(a => a.Name == attributeName);
            if (attribute == null)
                return null;

            return attribute.Value;
        }

        /// <summary>
        /// Returns value of the <paramref name="attribute"/> with line info updated.
        /// </summary>
        /// <param name="attribute">Attribute which value should be returned.</param>
        /// <returns>Value of the <paramref name="attribute"/> with line info updated.</returns>
        public static ISourceContent GetValue(this IXmlAttribute attribute)
        {
            Ensure.NotNull(attribute, "attribute");
            IDocumentPoint point;
            IDocumentPoint attributeLineInfo = attribute as IDocumentPoint;
            if (attributeLineInfo != null)
                point = new DefaultSourceLineInfo(attributeLineInfo.LineIndex, attributeLineInfo.ColumnIndex + attribute.Name.Length + 2);
            else
                point = new DefaultSourceLineInfo(0, 0);

            return new DefaultSourceContent(attribute.Value, point);
        }
    }
}
