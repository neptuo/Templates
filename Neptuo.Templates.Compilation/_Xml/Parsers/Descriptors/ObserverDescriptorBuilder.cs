using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public enum ObserverBuilderScope
    {
        /// <summary>
        /// One instance per attribute.
        /// </summary>
        PerAttribute,

        /// <summary>
        /// One instance per all attributes on component.
        /// </summary>
        PerElement,

        /// <summary>
        /// Singleton per document.
        /// </summary>
        PerDocument
    }

    /// <summary>
    /// Base builder for observer which operates on <see cref="IObserverDescriptor"/>.
    /// </summary>
    public abstract class ObserverDescriptorBuilder : IObserverBuilder
    {
        protected ObserverBuilderScope Scope { get; private set; }
        protected IPropertyBuilder PropertyFactory { get; private set; }

        public ObserverDescriptorBuilder(ObserverBuilderScope scope, IPropertyBuilder propertyFactory)
        {
            Guard.NotNull(propertyFactory, "propertyFactory");
            Scope = scope;
            PropertyFactory = propertyFactory;
        }

        public bool TryParse(IContentBuilderContext context, IComponentCodeObject codeObject, IXmlAttribute attribute)
        {
            switch (Scope)
            {
                case ObserverBuilderScope.PerAttribute:
                case ObserverBuilderScope.PerDocument:
                    // Just append observer.
                    break;
                case ObserverBuilderScope.PerElement:
                    // Check to update existing or append.
                    break;
            }

            //IObserverCodeObject observerObject = CreateCodeObject(context, attribute);
            //IObserverDescriptor observerDefinition = GetObserverDescriptor(context, codeObject, attribute);
            //codeObject.Observers.Add(observerObject);

            //TryBindProperties(context, new BindPropertiesContext(observerDefinition.GetProperties().ToDictionary(p => p.Name.ToLowerInvariant())), observerObject, attribute);
            return false;
        }

        protected abstract IObserverCodeObject CreateCodeObject(IContentBuilderContext context, IXmlAttribute attribute);
        protected abstract IObserverDescriptor GetObserverDescriptor(IContentBuilderContext context, IComponentCodeObject codeObject, IEnumerable<IXmlAttribute> attributes);

        protected abstract IPropertyDescriptor CreatePropertyDescriptor(IPropertyInfo propertyInfo);

        protected virtual bool TryBindProperties(IContentBuilderContext context, BindPropertiesContext bindContext, IObserverCodeObject codeObject, IEnumerable<IXmlAttribute> attributes)
        {
            // Bind attributes
            if (!TryBindAttributes(context, bindContext, codeObject, attributes))
                return false;

            // Process unbound attributes
            if (!ProcessUnboundAttributes(context, codeObject, bindContext.UnboundAttributes))
                return false;

            return true;
        }

        protected virtual bool TryBindAttributes(IContentBuilderContext context, BindPropertiesContext bindContext, IObserverCodeObject codeObject, IEnumerable<IXmlAttribute> attributes)
        {
            bool result = true;
            // Bind attributes
            foreach (IXmlAttribute attribute in attributes)
            {
                string attributeName = attribute.LocalName.ToLowerInvariant();
                IPropertyInfo propertyInfo;
                if (bindContext.Properties.TryGetValue(attributeName, out propertyInfo))
                {
                    bool boundAttribute = PropertyFactory.TryParse(context, codeObject, propertyInfo, attribute.GetValue());
                    if (boundAttribute)
                    {
                        bindContext.BoundProperies.Add(attributeName);
                        continue;
                    }

                    if (!boundAttribute)
                    {
                        bindContext.UnboundAttributes.Add(attribute);
                        result = false;
                    }
                }
                else
                {
                    bindContext.UnboundAttributes.Add(attribute);
                    return false;
                }
            }

            return result;
        }

        protected abstract bool ProcessUnboundAttributes(IContentBuilderContext context, IObserverCodeObject codeObject, List<IXmlAttribute> unboundAttributes);

    }
}
