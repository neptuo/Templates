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
        protected bool BindProperties(IContentBuilderContext context, IXmlElement element)
        {
            // Bind attributes
            ICollection<IXmlAttribute> unboundAttributes = new List<IXmlAttribute>();
            if (!BindPropertiesFromAttributes(context, element.Attributes, unboundAttributes))
                return false;

            // Bind inner elements
            ICollection<IXmlNode> unboundNodes = new List<IXmlNode>();
            if (!BindProperiesFromNodes(context, element.ChildNodes, unboundNodes))
                return false;

            // Process unbound attributes
            if (!ProcessUnboundAttributes(context, unboundAttributes))
                return false;

            // Bind content elements
            if (!ProcessUnboundNodes(context, unboundNodes))
                return false;

            return true;
        }

        /// <summary>
        /// Process xml attributes defined of xml element that is being processed.
        /// </summary>
        /// <param name="context">Context of current build.</param>
        /// <param name="attributes">Enumeration of defined xml attributes.</param>
        /// <param name="unboundAttributes">Collection of xml attributes that can't be used as properties.</param>
        /// <returns>Whether processing was successfull.</returns>
        private bool BindPropertiesFromAttributes(IContentBuilderContext context, IEnumerable<IXmlAttribute> attributes, ICollection<IXmlAttribute> unboundAttributes)
        {
            foreach (IXmlAttribute attribute in attributes)
            {
                string prefix = attribute.Prefix.ToLowerInvariant();
                string name = attribute.Name.ToLowerInvariant();
                ISourceContent value = attribute.GetValue();
                if (!TryBindProperty(context, prefix, name, value))
                    unboundAttributes.Add(attribute);
            }

            return true;
        }

        /// <summary>
        /// Process xml inner nodes of xml element that is being processed.
        /// </summary>
        /// <param name="context">Context of current build.</param>
        /// <param name="childNodes">Enumeration of inner xml nodes.</param>
        /// <param name="unboundNodes">Collection of inner xml nodes that can't be used as properties.</param>
        /// <returns>Whether processing was successfull.</returns>
        private bool BindProperiesFromNodes(IContentBuilderContext context, IEnumerable<IXmlNode> childNodes, ICollection<IXmlNode> unboundNodes)
        {
            bool isPropertyBound = false;
            foreach (IXmlNode childNode in childNodes)
            {
                if (childNode.NodeType == XmlNodeType.Element)
                {
                    IXmlElement element = (IXmlElement)childNode;
                    string prefix = element.Prefix.ToLowerInvariant();
                    string name = element.Name.ToLowerInvariant();
                    if (TryBindProperty(context, prefix, name, element.ChildNodes))
                    {
                        isPropertyBound = true;
                        break;
                    }
                }

                unboundNodes.Add(childNode);
            }

            IXmlNode firstNode = unboundNodes.FirstOrDefault();
            if (isPropertyBound && firstNode != null)
            {
                context.AddError(firstNode, "Oncy the component has defined any property as inner element, event default property must be wrapper in property element.");
                return false;
            }

            return true;
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
        protected abstract bool TryBindProperty(IContentBuilderContext context, string prefix, string name, ISourceContent value);

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
        /// <returns>Whether processing was successfull.</returns>
        protected virtual bool ProcessUnboundAttributes(IContentBuilderContext context, IEnumerable<IXmlAttribute> unboundAttributes)
        {
            bool result = true;
            foreach (IXmlAttribute attribute in unboundAttributes)
            {
                if (!ProcessUnboundAttribute(context, attribute))
                    result = false;
            }

            return result;
        }

        /// <summary>
        /// Should process xml attribute that isn't property neither observer.
        /// </summary>
        protected virtual bool ProcessUnboundAttribute(IContentBuilderContext context, IXmlAttribute unboundAttribute)
        {
            context.AddError(unboundAttribute, "Not recognized attribute.");
            return false;
        }

        /// <summary>
        /// All inner xml nodes that were not successfully bound using <see cref="ComponentBuilder.TryBindProperty"/> are passed to this method.
        /// </summary>
        /// <param name="context">Context of current build.</param>
        /// <param name="unboundNodes">Enumeration of unbound inner xml nodes.</param>
        /// <returns>Whether processing was successfull.</returns>
        protected virtual bool ProcessUnboundNodes(IContentBuilderContext context, IEnumerable<IXmlNode> unboundNodes)
        {
            bool result = true;
            foreach (IXmlNode node in unboundNodes)
            {
                if (!ProcessUnboundNode(context, node))
                    result = false;
            }

            return result;
        }

        /// <summary>
        /// Should process xml node that isn't property neither observer.
        /// </summary>
        protected virtual bool ProcessUnboundNode(IContentBuilderContext context, IXmlNode unboundNode)
        {
            context.AddError(unboundNode, "Not recognized node.");
            return false;
        }
    }
}