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

        protected virtual bool TryBindProperty(IContentBuilderContext context, BindPropertiesContext bindContext, IObserverCodeObject codeObject, IXmlAttribute attribute)
        {
            // Bind attribute
            if (!TryBindAttribute(context, bindContext, codeObject, attribute))
                return false;

            // Process unbound attribute
            if (!ProcessUnboundAttribute(context, codeObject, attribute))
                return false;

            return true;
        }

        protected virtual bool TryBindAttribute(IContentBuilderContext context, BindPropertiesContext bindContext, IObserverCodeObject codeObject, IXmlAttribute attribute)
        {
            string attributeName = attribute.LocalName.ToLowerInvariant();
            IPropertyInfo propertyInfo;
            if (bindContext.Properties.TryGetValue(attributeName, out propertyInfo))
            {
                bool boundAttribute = PropertyFactory.TryParse(context, codeObject, propertyInfo, attribute.GetValue());
                if (boundAttribute)
                {
                    bindContext.BoundProperies.Add(attributeName);
                    return true;
                }

            }

            bindContext.UnboundAttributes.Add(attribute);
            return false;
        }

        protected abstract bool ProcessUnboundAttribute(IContentBuilderContext context, IObserverCodeObject codeObject, IXmlAttribute unboundAttribute);
    }
}
