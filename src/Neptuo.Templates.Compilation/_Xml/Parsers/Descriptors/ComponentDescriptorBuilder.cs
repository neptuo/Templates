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
        private bool isParsing;

        protected IComponentCodeObject CodeObject { get; private set; }
        protected IComponentDescriptor ComponentDefinition { get; private set; }
        protected IPropertyInfo DefaultProperty { get; private set; }
        protected BindPropertiesContext BindContext { get; private set; }
        protected IPropertyBuilder PropertyFactory { get; private set; }
        protected IObserverBuilder ObserverFactory { get; private set; }

        public ComponentDescriptorBuilder(IPropertyBuilder propertyFactory, IObserverBuilder observerFactory)
        {
            Guard.NotNull(propertyFactory, "propertyFactory");
            Guard.NotNull(observerFactory, "observerFactory");
            PropertyFactory = propertyFactory;
            ObserverFactory = observerFactory;
        }

        public override IEnumerable<ICodeObject> TryParse(IContentBuilderContext context, IXmlElement element)
        {
            if (isParsing)
                throw Guard.Exception.InvalidOperation("ComponentDescriptorBuilder can't be reused! Create new instance for every xml tag.");

            isParsing = true;
            CodeObject = CreateCodeObject(context, element);
            ComponentDefinition = GetComponentDescriptor(context, CodeObject, element);
            DefaultProperty = ComponentDefinition.GetDefaultProperty();
            BindContext = new BindPropertiesContext(ComponentDefinition.GetProperties().ToDictionary(p => p.Name.ToLowerInvariant()));

            BindProperties(context, element);
            isParsing = false;
            return new List<ICodeObject> { CodeObject };
        }

        protected override bool TryBindProperty(IContentBuilderContext context, string prefix, string name, ISourceContent value)
        {
            IPropertyInfo propertyInfo;
            if (BindContext.Properties.TryGetValue(name, out propertyInfo))
            {
                bool result = PropertyFactory.TryParse(context, CodeObject, propertyInfo, value);
                if (result)
                {
                    BindContext.BoundProperies.Add(name);
                    return true;
                }
            }

            return false;
        }

        protected override bool TryBindProperty(IContentBuilderContext context, string prefix, string name, IEnumerable<IXmlNode> value)
        {
            IPropertyInfo propertyInfo;
            if (!BindContext.BoundProperies.Contains(name) && BindContext.Properties.TryGetValue(name, out propertyInfo))
            {
                bool result = PropertyFactory.TryParse(context, CodeObject, propertyInfo, value);
                if (result)
                {
                    BindContext.BoundProperies.Add(name);
                    BindContext.IsBoundFromContent = true;
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
                if (!boundAttribute && ObserverFactory.TryParse(context, CodeObject, attribute))
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
                if (BindContext.IsBoundFromContent)
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
                if (DefaultProperty != null && !BindContext.BoundProperies.Contains(DefaultProperty.Name.ToLowerInvariant()))
                {
                    bool result = PropertyFactory.TryParse(context, CodeObject, DefaultProperty, unboundNodes);
                    if (result)
                    {
                        BindContext.BoundProperies.Add(DefaultProperty.Name);
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