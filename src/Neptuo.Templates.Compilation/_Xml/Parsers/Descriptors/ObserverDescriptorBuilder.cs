using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Base builder for observer which operates on <see cref="IObserverDescriptor"/>.
    /// </summary>
    public abstract class ObserverDescriptorBuilder : IObserverBuilder
    {
        protected IPropertyBuilder PropertyFactory { get; private set; }

        public ObserverDescriptorBuilder(IPropertyBuilder propertyFactory)
        {
            Guard.NotNull(propertyFactory, "propertyFactory");
            PropertyFactory = propertyFactory;
        }

        protected abstract IObserverCodeObject IsObserverContained(IContentBuilderContext context, IComponentCodeObject codeObject, IXmlAttribute attribute);

        public bool TryParse(IContentBuilderContext context, IComponentCodeObject codeObject, IXmlAttribute attribute)
        {
            IObserverCodeObject observerObject = IsObserverContained(context, codeObject, attribute);
            if (observerObject != null)
                return TryUpdateObserver(context, codeObject, observerObject, attribute);
            else
                return TryAppendNewObserver(context, codeObject, attribute);
        }

        protected abstract IObserverCodeObject CreateCodeObject(IContentBuilderContext context, IXmlAttribute attribute);

        protected bool TryAppendNewObserver(IContentBuilderContext context, IComponentCodeObject codeObject, IXmlAttribute attribute)
        {
            IObserverCodeObject observerObject = CreateCodeObject(context, attribute);
            IComponentDescriptor observerDescriptor = GetObserverDescriptor(context, codeObject, attribute);
            codeObject.Observers.Add(observerObject);
            return TryBindProperty(context, new BindContentPropertiesContext(observerDescriptor), observerObject, attribute);
        }

        protected bool TryUpdateObserver(IContentBuilderContext context, IComponentCodeObject codeObject, IObserverCodeObject observerObject, IXmlAttribute attribute)
        {
            IComponentDescriptor observerDescriptor = GetObserverDescriptor(context, codeObject, attribute);
            return TryBindProperty(context, new BindContentPropertiesContext(observerDescriptor, observerObject), observerObject, attribute);
        }

        protected abstract IComponentDescriptor GetObserverDescriptor(IContentBuilderContext context, IComponentCodeObject codeObject, IXmlAttribute attribute);

        protected virtual bool TryBindProperty(IContentBuilderContext context, BindContentPropertiesContext bindContext, IObserverCodeObject codeObject, IXmlAttribute attribute)
        {
            // Bind attribute
            if (TryBindAttribute(context, bindContext, codeObject, attribute))
                return true;

            // Process unbound attribute
            if (ProcessUnboundAttribute(context, codeObject, attribute))
                return true;

            return false;
        }

        protected virtual bool TryBindAttribute(IContentBuilderContext context, BindContentPropertiesContext bindContext, IObserverCodeObject codeObject, IXmlAttribute attribute)
        {
            string attributeName = attribute.LocalName.ToLowerInvariant();
            IPropertyInfo propertyInfo;
            if (bindContext.Properties.TryGetValue(attributeName, out propertyInfo))
            {
                IEnumerable<ICodeProperty> codeProperties = context.TryProcessProperty(PropertyFactory, propertyInfo, attribute.GetValue());
                if(codeProperties != null)
                {
                    codeObject.Properties.AddRange(codeProperties);
                    bindContext.BoundProperties.Add(attributeName);
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
