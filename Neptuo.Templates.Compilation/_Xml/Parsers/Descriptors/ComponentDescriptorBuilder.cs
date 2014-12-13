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

        public override ICodeObject TryParse(IContentBuilderContext context, IXmlElement element)
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
            return CodeObject;
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
                ResolvePropertyValue(context, CodeObject, propertyInfo, value);
                BindContext.BoundProperies.Add(name);
                return true;
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
                // Bind content elements
                if (DefaultProperty != null && !BindContext.BoundProperies.Contains(DefaultProperty.Name.ToLowerInvariant()))
                    return ResolvePropertyValue(context, CodeObject, DefaultProperty, unboundNodes);

                return false;
            }

            return true;
        }

        /// <summary>
        /// Should create code object for this component.
        /// </summary>
        protected abstract IComponentCodeObject CreateCodeObject(IContentBuilderContext context, IXmlElement element);

        /// <summary>
        /// Gets current component definition.
        /// </summary>
        protected abstract IComponentDescriptor GetComponentDescriptor(IContentBuilderContext context, IComponentCodeObject codeObject, IXmlElement element);

        protected virtual bool ResolvePropertyValue(IContentBuilderContext context, IPropertiesCodeObject codeObject, IPropertyInfo propertyInfo, IEnumerable<IXmlNode> content)
        {
            bool result = PropertyFactory.TryParse(context, codeObject, propertyInfo, content);
            if (result)
                return true;

            return false;
        }
    }
}