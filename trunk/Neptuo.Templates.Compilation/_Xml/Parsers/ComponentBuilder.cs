using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public abstract class ComponentBuilder : IComponentBuilder
    {
        public virtual void Parse(IContentBuilderContext context, IXmlElement element)
        {
            BindProperties(context, element);
        }

        /// <summary>
        /// Process <paramref name="element"/>.
        /// </summary>
        /// <param name="context">Context of current build.</param>
        /// <param name="element">Xml element that is being processed.</param>
        protected virtual void BindProperties(IContentBuilderContext context, IXmlElement element)
        {
            List<IXmlAttribute> unboundAttributes = new List<IXmlAttribute>();
            List<IXmlNode> unboundNodes = new List<IXmlNode>();

            // Bind attributes
            BindAttributes(context, element.Attributes, unboundAttributes);
            
            // Bind inner elements
            BindNodes(context, element.ChildNodes, unboundNodes);

            // Process unbound attributes
            ProcessUnboundAttributes(context, unboundAttributes);

            // Bind content elements
            ProcessUnboundNodes(context, unboundNodes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attributes"></param>
        /// <param name="unboundAttributes"></param>
        protected virtual void BindAttributes(IContentBuilderContext context, IEnumerable<IXmlAttribute> attributes, List<IXmlAttribute> unboundAttributes)
        {
            // Bind attributes
            foreach (IXmlAttribute attribute in attributes)
            {
                if (!TryBindProperty(context, attribute.Prefix.ToLowerInvariant(), attribute.Name.ToLowerInvariant(), attribute.Value))
                    unboundAttributes.Add(attribute);
            }
        }

        protected virtual void BindNodes(IContentBuilderContext context, IEnumerable<IXmlNode> childNodes, List<IXmlNode> unboundNodes)
        {
            // Bind inner elements
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
        /// <param name="unboundAttributes">Enumeration of unbound inner xml nodes.</param>
        protected virtual void ProcessUnboundNodes(IContentBuilderContext context, IEnumerable<IXmlNode> unboundNodes)
        { }
    }
}