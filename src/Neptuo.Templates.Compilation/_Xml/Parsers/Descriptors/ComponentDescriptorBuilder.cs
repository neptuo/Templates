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
    /// Base component builder including logic for processing XML elements as properties and inner values.
    /// 
    /// </summary>
    public abstract class ComponentDescriptorBuilder : ComponentBuilder
    {
        protected IContentPropertyBuilder PropertyFactory { get; private set; }
        protected IObserverBuilder ObserverFactory { get; private set; }

        /// <summary>
        /// Creates new instance with <paramref name="propertyFactory"/> as builder for resolving property values 
        /// and <paramref name="observerFactory"/> as builder for resolving observers.
        /// </summary>
        /// <param name="propertyFactory">Builder for resolving property values.</param>
        /// <param name="observerFactory">Builder for resolving observers.</param>
        public ComponentDescriptorBuilder(IContentPropertyBuilder propertyFactory, IObserverBuilder observerFactory)
        {
            Guard.NotNull(propertyFactory, "propertyFactory");
            Guard.NotNull(observerFactory, "observerFactory");
            PropertyFactory = propertyFactory;
            ObserverFactory = observerFactory;
        }

        public override IEnumerable<ICodeObject> TryParse(IContentBuilderContext context, IXmlElement element)
        {
            IComponentCodeObject codeObject = CreateCodeObject(context, element);
            IComponentDescriptor componentDefinition = GetComponentDescriptor(context, codeObject, element);
            IPropertyInfo defaultProperty = componentDefinition.GetDefaultProperty();
            BindContentPropertiesContext bindContext = new BindContentPropertiesContext(componentDefinition.GetProperties().ToDictionary(p => p.Name.ToLowerInvariant()));
            context.ComponentCodeObject(codeObject);
            context.ComponentDescriptor(componentDefinition);
            context.BindPropertiesContext(bindContext);
            context.DefaultProperty(defaultProperty);

            BindProperties(context, element);
            return new List<ICodeObject> { codeObject };
        }

        protected override bool TryBindProperty(IContentBuilderContext context, string prefix, string name, ISourceContent value)
        {
            IPropertyInfo propertyInfo;
            if (context.BindPropertiesContext().Properties.TryGetValue(name, out propertyInfo))
            {
                IEnumerable<ICodeProperty> codeProperties = context.TryProcessProperty(PropertyFactory, propertyInfo, value);
                if (codeProperties != null)
                {
                    context.ComponentCodeObject().Properties.AddRange(codeProperties);
                    context.BindPropertiesContext().BoundProperties.Add(name);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Tries to find property named <paramref name="prefix"/> + <paramref name="name"/> and delegates parsing it's value to property factory.
        /// </summary>
        protected override bool TryBindProperty(IContentBuilderContext context, string prefix, string name, IEnumerable<IXmlNode> value)
        {
            IPropertyInfo propertyInfo;
            if (!context.BindPropertiesContext().BoundProperties.Contains(name) && context.BindPropertiesContext().Properties.TryGetValue(name, out propertyInfo))
            {
                IEnumerable<ICodeProperty> codeProperties = context.TryProcessProperty(PropertyFactory, propertyInfo, value);
                if (codeProperties != null)
                {
                    context.ComponentCodeObject().Properties.AddRange(codeProperties);
                    context.BindPropertiesContext().BoundProperties.Add(name);
                    context.BindPropertiesContext().IsBoundFromContent = true;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Skips xmlns definitions, other attributes tries to parse as observers; otherwise marks those as errors.
        /// </summary>
        protected override bool ProcessUnboundAttributes(IContentBuilderContext context, IEnumerable<IXmlAttribute> unboundAttributes)
        {
            bool result = true;
            List<IXmlAttribute> usedAttributes = new List<IXmlAttribute>();
            foreach (IXmlAttribute attribute in unboundAttributes)
            {
                bool boundAttribute = false;

                // TODO: Process xmlns registration.
                if (attribute.Prefix.ToLowerInvariant() == "xmlns")
                    boundAttribute = true;

                // Try process as observer.
                if (!boundAttribute && ObserverFactory.TryParse(context, context.ComponentCodeObject(), attribute))
                    boundAttribute = true;

                // Call base if attribute was not bound.
                if (!boundAttribute && !ProcessUnboundAttribute(context, attribute))
                    result = false;
            }

            return result;
        }

        /// <summary>
        /// Tries to bind default property from <paramref name="unboundNodes"/>.
        /// </summary>
        protected override bool ProcessUnboundNodes(IContentBuilderContext context, IEnumerable<IXmlNode> unboundNodes)
        {
            if (unboundNodes.Any())
            {
                // If at least one property was bound from content element, default property is not supported.
                if (context.BindPropertiesContext().IsBoundFromContent)
                {
                    IXmlNode node = FindFirstSignificantNode(unboundNodes);
                    if (node != null)
                    {
                        context.AddError(node, "If at least one property was bound from content element, default property is not supported.");
                        return false;
                    }

                    return true;
                }

                // Bind content elements
                IPropertyInfo defaultProperty = context.DefaultProperty();
                if (defaultProperty != null && !context.BindPropertiesContext().BoundProperties.Contains(defaultProperty.Name.ToLowerInvariant()))
                {
                    IEnumerable<ICodeProperty> codeProperties = context.TryProcessProperty(PropertyFactory, defaultProperty, unboundNodes);
                    if (codeProperties != null)
                    {
                        context.ComponentCodeObject().Properties.AddRange(codeProperties);
                        context.BindPropertiesContext().BoundProperties.Add(context.DefaultProperty().Name);
                        return true;
                    }
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns first not white space node from <paramref name="nodes"/>; otherwise <c>null</c>;
        /// </summary>
        /// <param name="nodes">Enumeration of nodes to search.</param>
        /// <returns>First not white space node from <paramref name="nodes"/>; otherwise <c>null</c>;</returns>
        protected IXmlNode FindFirstSignificantNode(IEnumerable<IXmlNode> nodes)
        {
            IXmlNode node = nodes.FirstOrDefault(n => n.NodeType == XmlNodeType.Element);
            if (node == null)
                node = nodes.OfType<IXmlText>().FirstOrDefault(t => !String.IsNullOrEmpty(t.Text) && !String.IsNullOrWhiteSpace(t.Text));

            return node;
        }

        /// <summary>
        /// Should create code object for this component.
        /// </summary>
        protected abstract IComponentCodeObject CreateCodeObject(IContentBuilderContext context, IXmlElement element);

        /// <summary>
        /// Gets current component definition.
        /// </summary>
        protected abstract IComponentDescriptor GetComponentDescriptor(IContentBuilderContext context, IComponentCodeObject codeObject, IXmlElement element);
    }
}