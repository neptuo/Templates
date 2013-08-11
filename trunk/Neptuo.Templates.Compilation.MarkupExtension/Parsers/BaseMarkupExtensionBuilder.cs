using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers.ExtensionContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public abstract class BaseMarkupExtensionBuilder : IMarkupExtensionBuilder
    {
        protected abstract IValueExtensionCodeObject CreateCodeObject(IMarkupExtensionBuilderContext context, Extension extension);
        protected abstract IMarkupExtensionInfo GetExtensionDefinition(IMarkupExtensionBuilderContext context, IValueExtensionCodeObject codeObject, Extension extension);

        protected abstract IPropertyDescriptor CreateSetPropertyDescriptor(IPropertyInfo propertyInfo);
        protected abstract IPropertyDescriptor CreateListAddPropertyDescriptor(IPropertyInfo propertyInfo);

        public bool Parse(IMarkupExtensionBuilderContext context, Extension extension)
        {
            IValueExtensionCodeObject codeObject = CreateCodeObject(context, extension);
            if (codeObject == null)
                return false;

            context.Parent.SetValue(codeObject);
            return BindProperties(context, codeObject, extension);
        }

        protected virtual bool BindProperties(IMarkupExtensionBuilderContext context, IValueExtensionCodeObject codeObject, Extension extension)
        {
            HashSet<string> boundProperies = new HashSet<string>();
            IMarkupExtensionInfo extensionDefinition = GetExtensionDefinition(context, codeObject, extension);
            IPropertyInfo defaultProperty = extensionDefinition.GetDefaultProperty();

            foreach (IPropertyInfo propertyInfo in extensionDefinition.GetProperties())
            {
                string propertyName = propertyInfo.Name.ToLowerInvariant();
                foreach (ExtensionAttribute attribute in extension.Attributes)
                {
                    if (propertyName == attribute.Name.ToLowerInvariant())
                    {
                        IPropertyDescriptor propertyDescriptor = CreateSetPropertyDescriptor(propertyInfo);
                        bool result = context.ParserContext.ParserService.ProcessValue(
                            attribute.Value,
                            new DefaultParserServiceContext(context.ParserContext.DependencyProvider, propertyDescriptor, context.ParserContext.Errors)
                        );

                        if (result)
                        {
                            codeObject.Properties.Add(propertyDescriptor);
                            boundProperies.Add(propertyName);
                        }
                    }
                }
            }

            if (defaultProperty != null && !boundProperies.Contains(defaultProperty.Name.ToLowerInvariant()))
            {
                IPropertyDescriptor propertyDescriptor = CreateSetPropertyDescriptor(defaultProperty);
                if (!String.IsNullOrWhiteSpace(extension.DefaultAttributeValue))
                {
                    bool result = context.ParserContext.ParserService.ProcessValue(
                        extension.DefaultAttributeValue,
                        new DefaultParserServiceContext(context.ParserContext.DependencyProvider, propertyDescriptor, context.ParserContext.Errors)
                    );

                    if (result)
                        codeObject.Properties.Add(propertyDescriptor);
                }
            }

            return true;
        }
    }
}
