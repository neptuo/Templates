using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Base logic processing xml element.
    /// Attributes and inner nodes and processed as properties.
    /// </summary>
    public abstract class ComponentBuilder : IContentBuilder
    {
        /// <summary>
        /// Process <paramref name="element"/>.
        /// </summary>
        public abstract ICodeObject Parse(IContentBuilderContext context, IXmlElement element);

        /// <summary>
        /// Binds attributes and inner elements of <paramref name="element"/>.
        /// </summary>
        /// <param name="context">Context of current build.</param>
        /// <param name="element">Xml element that is being processed.</param>
        protected void BindProperties(IContentBuilderContext context, IXmlElement element)
        {
            // Bind attributes
            IEnumerable<IXmlAttribute> unboundAttributes = BindPropertiesFromAttributes(context, element.Attributes);

            // Bind inner elements
            IEnumerable<IXmlNode> unboundNodes = BindProperiesFromNodes(context, element.ChildNodes);

            // Process unbound attributes
            ProcessUnboundAttributes(context, unboundAttributes);

            // Bind content elements
            ProcessUnboundNodes(context, unboundNodes);
        }

        /// <summary>
        /// Process xml attributes defined of xml element that is being processed.
        /// </summary>
        /// <param name="context">Context of current build.</param>
        /// <param name="attributes">Enumeration of defined xml attributes.</param>
        /// <returns>Enumeration of xml attributes that can't be used as properties.</returns>
        private IEnumerable<IXmlAttribute> BindPropertiesFromAttributes(IContentBuilderContext context, IEnumerable<IXmlAttribute> attributes)
        {
            List<IXmlAttribute> unboundAttributes = new List<IXmlAttribute>();
            foreach (IXmlAttribute attribute in attributes)
            {
                if (!TryBindProperty(context, attribute.Prefix.ToLowerInvariant(), attribute.Name.ToLowerInvariant(), attribute.Value))
                    unboundAttributes.Add(attribute);
            }

            return unboundAttributes;
        }

        /// <summary>
        /// Process xml inner nodes of xml element that is being processed.
        /// </summary>
        /// <param name="context">Context of current build.</param>
        /// <param name="childNodes">Enumeration of inner xml nodes.</param>
        /// <returns>Enumeration of inner xml nodes that can't be used as properties.</returns>
        private IEnumerable<IXmlNode> BindProperiesFromNodes(IContentBuilderContext context, IEnumerable<IXmlNode> childNodes)
        {
            List<IXmlNode> unboundNodes = new List<IXmlNode>();
            foreach (IXmlNode childNode in childNodes)
            {
                if (childNode.NodeType == XmlNodeType.Element)
                {
                    IXmlElement element = (IXmlElement)childNode;
                    string childName = element.Name.ToLowerInvariant();
                    if (!TryBindProperty(context, element.Prefix.ToLowerInvariant(), element.Name.ToLowerInvariant(), element.ChildNodes))
                        unboundNodes.Add(element);
                }
            }

            return unboundNodes;
        }

        /// <summary>
        /// Should tries to bind property value from attribute named <paramref name="name"/> 
        /// prefixed with <paramref name="prefix"/> and value of <paramref name="value"/>.
        /// </summary>
        /// <param name="context">Context of current build.</param>
        /// <param name="prefix">Attribute prefix.</param>
        /// <param name="name">Attribute name.</param>
        /// <param name="value">Attribute value.</param>
        /// <returns><c>true</c> if bind was successfull; <c>false</c> otherwise.</returns>
        protected abstract bool TryBindProperty(IContentBuilderContext context, string prefix, string name, string value);

        /// <summary>
        /// Should tries to bind property value from inner element named <paramref name="name"/> prefixed with <paramref name="prefix"/> and content of <paramref name="value"/>.
        /// </summary>
        /// <param name="context">Context of current build.</param>
        /// <param name="prefix">Inner element prefix.</param>
        /// <param name="name">Inner element name.</param>
        /// <param name="value">Inner element content.</param>
        /// <returns><c>true</c> if bind was successfull; <c>false</c> otherwise.</returns>
        protected abstract bool TryBindProperty(IContentBuilderContext context, string prefix, string name, IEnumerable<IXmlNode> value);

        /// <summary>
        /// All attributes that were not successfully bound using <see cref="ComponentBuilder.TryBindProperty"/> are passed to this method.
        /// </summary>
        /// <param name="context">Context of current build.</param>
        /// <param name="unboundAttributes">Enumeration of unbound attributes.</param>
        protected virtual void ProcessUnboundAttributes(IContentBuilderContext context, IEnumerable<IXmlAttribute> unboundAttributes)
        { }

        /// <summary>
        /// All inner xml nodes that were not successfully bound using <see cref="ComponentBuilder.TryBindProperty"/> are passed to this method.
        /// </summary>
        /// <param name="context">Context of current build.</param>
        /// <param name="unboundNodes">Enumeration of unbound inner xml nodes.</param>
        protected virtual void ProcessUnboundNodes(IContentBuilderContext context, IEnumerable<IXmlNode> unboundNodes)
        { }
    }
}