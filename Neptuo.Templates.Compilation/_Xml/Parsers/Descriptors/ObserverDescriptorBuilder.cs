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

        protected abstract IObserverCodeObject IsObserverContained(IContentBuilderContext context, IComponentCodeObject codeObject, IXmlAttribute attribute);

        public bool TryParse(IContentBuilderContext context, IComponentCodeObject codeObject, IXmlAttribute attribute)
        {
            if (Scope == ObserverBuilderScope.PerElement)
            {
                IObserverCodeObject observerObject = IsObserverContained(context, codeObject, attribute);
                if (observerObject != null)
                    return TryUpdateObserver(context, codeObject, observerObject, attribute);
                else
                    return TryAppendNewObserver(context, codeObject, attribute);
            }

            if (Scope == ObserverBuilderScope.PerAttribute || Scope == ObserverBuilderScope.PerDocument)
                return TryAppendNewObserver(context, codeObject, attribute);

            throw new NotSupportedException(Scope.ToString());
        }

        protected abstract IObserverCodeObject CreateCodeObject(IContentBuilderContext context, IXmlAttribute attribute);

        protected bool TryAppendNewObserver(IContentBuilderContext context, IComponentCodeObject codeObject, IXmlAttribute attribute)
        {
            IObserverCodeObject observerObject = CreateCodeObject(context, attribute);
            IObserverDescriptor observerDescriptor = GetObserverDescriptor(context, codeObject, attribute);
            codeObject.Observers.Add(observerObject);
            return TryBindProperty(context, new BindPropertiesContext(observerDescriptor), observerObject, attribute);
        }

        protected bool TryUpdateObserver(IContentBuilderContext context, IComponentCodeObject codeObject, IObserverCodeObject observerObject, IXmlAttribute attribute)
        {
            IObserverDescriptor observerDescriptor = GetObserverDescriptor(context, codeObject, attribute);
            return TryBindProperty(context, new BindPropertiesContext(observerDescriptor, observerObject), observerObject, attribute);
        }

        protected abstract IObserverDescriptor GetObserverDescriptor(IContentBuilderContext context, IComponentCodeObject codeObject, IXmlAttribute attribute);

        protected virtual bool TryBindProperty(IContentBuilderContext context, BindPropertiesContext bindContext, IObserverCodeObject codeObject, IXmlAttribute attribute)
        {
            // Bind attribute
            if (TryBindAttribute(context, bindContext, codeObject, attribute))
                return true;

            // Process unbound attribute
            if (ProcessUnboundAttribute(context, codeObject, attribute))
                return true;

            return false;
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

        protected virtual bool ProcessUnboundAttribute(IContentBuilderContext context, IObserverCodeObject codeObject, IXmlAttribute unboundAttribute)
        {
            context.AddError(unboundAttribute, "Unnable to attach attribute to observer.");
            return false;
        }
    }
}
