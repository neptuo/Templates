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
    /// </summary>
    public abstract class ComponentDescriptorBuilder : ComponentBuilder
    {
        protected IPropertyBuilder PropertyFactory { get; private set; }
        protected IContentPropertyBuilder ContentPropertyFactory { get; private set; }
        protected IObserverBuilder ObserverFactory { get; private set; }

        public ComponentDescriptorBuilder(IPropertyBuilder propertyFactory, IContentPropertyBuilder contentPropertyFactory, IObserverBuilder observerFactory)
        {
            Guard.NotNull(propertyFactory, "propertyFactory");
            Guard.NotNull(contentPropertyFactory, "contentPropertyFactory");
            Guard.NotNull(observerFactory, "observerFactory");
            PropertyFactory = propertyFactory;
            ContentPropertyFactory = contentPropertyFactory;
            ObserverFactory = observerFactory;
        }

        public override IEnumerable<ICodeObject> TryParse(IContentBuilderContext context, IXmlElement element)
        {
            IComponentCodeObject codeObject = CreateCodeObject(context, element);
            IComponentDescriptor componentDefinition = GetComponentDescriptor(context, codeObject, element);
            IPropertyInfo defaultProperty = componentDefinition.GetDefaultProperty();
            BindPropertiesContext bindContext = new BindPropertiesContext(componentDefinition.GetProperties().ToDictionary(p => p.Name.ToLowerInvariant()));
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
                IEnumerable<IPropertyDescriptor> propertyDescriptors = context.TryProcessProperty(PropertyFactory, propertyInfo, value);
                if (propertyDescriptors != null)
                {
                    context.ComponentCodeObject().Properties.AddRange(propertyDescriptors);
                    context.BindPropertiesContext().BoundProperies.Add(name);
                    return true;
                }
            }

            return false;
        }

        protected override bool TryBindProperty(IContentBuilderContext context, string prefix, string name, IEnumerable<IXmlNode> value)
        {
            IPropertyInfo propertyInfo;
            if (!context.BindPropertiesContext().BoundProperies.Contains(name) && context.BindPropertiesContext().Properties.TryGetValue(name, out propertyInfo))
            {
                IEnumerable<IPropertyDescriptor> propertyDescriptors = context.TryProcessProperty(ContentPropertyFactory, propertyInfo, value);
                if (propertyDescriptors != null)
                {
                    context.ComponentCodeObject().Properties.AddRange(propertyDescriptors);
                    context.BindPropertiesContext().BoundProperies.Add(name);
                    context.BindPropertiesContext().IsBoundFromContent = true;
                    return true;
                }
            }

            return false;
        }

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
                if (defaultProperty != null && !context.BindPropertiesContext().BoundProperies.Contains(defaultProperty.Name.ToLowerInvariant()))
                {
                    IEnumerable<IPropertyDescriptor> propertyDescriptors = context.TryProcessProperty(ContentPropertyFactory, defaultProperty, unboundNodes);
                    if (propertyDescriptors != null)
                    {
                        context.ComponentCodeObject().Properties.AddRange(propertyDescriptors);
                        context.BindPropertiesContext().BoundProperies.Add(context.DefaultProperty().Name);
                        return true;
                    }
                }

                return false;
            }

            return true;
        }

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