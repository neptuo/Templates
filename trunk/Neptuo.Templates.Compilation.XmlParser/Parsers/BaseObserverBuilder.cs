using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public abstract class BaseObserverBuilder : IObserverBuilder
    {
        public void Parse(IContentBuilderContext context, IComponentCodeObject codeObject, IEnumerable<IXmlAttribute> attributes)
        {
            IObserverCodeObject observerObject = CreateCodeObject(context, attributes);
            IObserverInfo observerDefinition = GetObserverDefinition(context, codeObject, attributes);
            codeObject.Observers.Add(observerObject);

            BindProperties(context, new BindPropertiesContext(observerDefinition.GetProperties().ToDictionary(p => p.Name.ToLowerInvariant())), observerObject, attributes);
        }

        protected abstract IObserverCodeObject CreateCodeObject(IContentBuilderContext context, IEnumerable<IXmlAttribute> attributes);
        protected abstract IObserverInfo GetObserverDefinition(IContentBuilderContext context, IComponentCodeObject codeObject, IEnumerable<IXmlAttribute> attributes);

        protected abstract IPropertyDescriptor CreateSetPropertyDescriptor(IPropertyInfo propertyInfo);
        protected abstract IPropertyDescriptor CreateListAddPropertyDescriptor(IPropertyInfo propertyInfo);

        protected virtual void BindProperties(IContentBuilderContext context, BindPropertiesContext bindContext, IObserverCodeObject codeObject, IEnumerable<IXmlAttribute> attributes)
        {
            // Bind attributes
            BindAttributes(context, bindContext, codeObject, attributes);

            // Process unbound attributes
            ProcessUnboundAttributes(context, codeObject, bindContext.UnboundAttributes);
        }

        protected virtual void BindAttributes(IContentBuilderContext context, BindPropertiesContext bindContext, IObserverCodeObject codeObject, IEnumerable<IXmlAttribute> attributes)
        {
            // Bind attributes
            foreach (IXmlAttribute attribute in attributes)
            {
                string attributeName = attribute.LocalName.ToLowerInvariant();
                IPropertyInfo propertyInfo;
                if (bindContext.Properties.TryGetValue(attributeName, out propertyInfo))
                {
                    IPropertyDescriptor propertyDescriptor = CreateSetPropertyDescriptor(propertyInfo);
                    bool result = context.ParserContext.ParserService.ProcessValue(
                        attribute.Value,
                        new DefaultParserServiceContext(context.ParserContext.DependencyProvider, propertyDescriptor, context.ParserContext.Errors)
                    );

                    if (result)
                    {
                        codeObject.Properties.Add(propertyDescriptor);
                        bindContext.BoundProperies.Add(attributeName);
                    }
                    else
                    {
                        bindContext.UnboundAttributes.Add(attribute);
                    }
                }
                else
                {
                    bindContext.UnboundAttributes.Add(attribute);
                }
            }
        }

        protected abstract void ProcessUnboundAttributes(IContentBuilderContext context, IObserverCodeObject codeObject, List<IXmlAttribute> unboundAttributes);
    }
}
