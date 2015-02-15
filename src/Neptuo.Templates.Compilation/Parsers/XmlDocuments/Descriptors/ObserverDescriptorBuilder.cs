using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.XmlDocuments
{
    /// <summary>
    /// Base builder for observer which operates on <see cref="IComponentDescriptor"/>.
    /// </summary>
    public abstract class ObserverDescriptorBuilder : IObserverBuilder
    {
        /// <summary>
        /// Should return not null value to indicate that <paramref name="attribute"/> should be attached to existing observer.
        /// If returns <c>null</c>, new observer will be created and attached.
        /// </summary>
        /// <param name="context">Current building context.</param>
        /// <param name="codeObject">Observer source component.</param>
        /// <param name="attribute">Attribute to process.</param>
        /// <returns>
        /// Should return not null value to indicate that <paramref name="attribute"/> should be attached to existing observer.
        /// If returns <c>null</c>, new observer will be created and attached.
        /// </returns>
        protected abstract ICodeObject IsObserverContained(IContentBuilderContext context, IObserversCodeObject codeObject, IXmlAttribute attribute);

        protected abstract ICodeObject CreateCodeObject(IContentBuilderContext context, IXmlAttribute attribute);

        protected abstract IComponentDescriptor GetObserverDescriptor(IContentBuilderContext context, IObserversCodeObject codeObject, IXmlAttribute attribute);

        public bool TryParse(IContentBuilderContext context, IObserversCodeObject codeObject, IXmlAttribute attribute)
        {
            BindContentPropertiesContext bindContext;
            IComponentDescriptor observerDescriptor = GetObserverDescriptor(context, codeObject, attribute);

            // Create new observer or update existing?
            IPropertiesCodeObject observerObject = (IPropertiesCodeObject)IsObserverContained(context, codeObject, attribute);
            if (observerObject != null)
            {
                bindContext = new BindContentPropertiesContext(observerDescriptor, context.Registry.WithPropertyNormalizer());
            }
            else
            {
                observerObject = (IPropertiesCodeObject)CreateCodeObject(context, attribute);
                codeObject.Observers.Add(observerObject);
                bindContext = new BindContentPropertiesContext(observerDescriptor, context.Registry.WithPropertyNormalizer());
            }

            // Bind attribute
            if (TryBindProperty(context, bindContext, observerObject, attribute))
                return true;

            // Process unbound attribute
            if (ProcessUnboundAttribute(context, observerObject, attribute))
                return true;

            return false;
        }

        protected virtual bool TryBindProperty(IContentBuilderContext context, BindContentPropertiesContext bindContext, IPropertiesCodeObject codeObject, IXmlAttribute attribute)
        {
            string attributeName = context.Registry.WithPropertyNormalizer().PrepareName(attribute.LocalName);
            IPropertyInfo propertyInfo;
            if (bindContext.Properties.TryGetValue(attributeName, out propertyInfo))
            {
                IEnumerable<ICodeProperty> codeProperties = context.TryProcessProperty(propertyInfo, attribute.GetValue());
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

        protected virtual bool ProcessUnboundAttribute(IContentBuilderContext context, IPropertiesCodeObject codeObject, IXmlAttribute unboundAttribute)
        {
            context.AddError(unboundAttribute, "Unnable to attach attribute to observer.");
            return false;
        }
    }
}
