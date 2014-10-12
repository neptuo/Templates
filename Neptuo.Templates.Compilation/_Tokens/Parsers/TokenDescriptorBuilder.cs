using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Base implementation of <see cref="ITokenBuilder"/> that uses <see cref="ITokenDescriptor"/> as target decriptor.
    /// </summary>
    public abstract class TokenDescriptorBuilder : ITokenBuilder
    {
        protected abstract IValueExtensionCodeObject CreateCodeObject(ITokenBuilderContext context, Token extension);
        protected abstract ITokenDescriptor GetExtensionDefinition(ITokenBuilderContext context, IValueExtensionCodeObject codeObject, Token extension);

        protected abstract IPropertyDescriptor CreateSetPropertyDescriptor(IPropertyInfo propertyInfo);
        protected abstract IPropertyDescriptor CreateListAddPropertyDescriptor(IPropertyInfo propertyInfo);

        public ICodeObject Parse(ITokenBuilderContext context, Token extension)
        {
            IValueExtensionCodeObject codeObject = CreateCodeObject(context, extension);
            if (codeObject != null && BindProperties(context, codeObject, extension))
                return codeObject;

            return null;
        }

        protected virtual bool BindProperties(ITokenBuilderContext context, IValueExtensionCodeObject codeObject, Token extension)
        {
            HashSet<string> boundProperies = new HashSet<string>();
            ITokenDescriptor extensionDefinition = GetExtensionDefinition(context, codeObject, extension);
            IPropertyInfo defaultProperty = extensionDefinition.GetDefaultProperty();

            foreach (IPropertyInfo propertyInfo in extensionDefinition.GetProperties())
            {
                string propertyName = propertyInfo.Name.ToLowerInvariant();
                foreach (TokenAttribute attribute in extension.Attributes)
                {
                    if (propertyName == attribute.Name.ToLowerInvariant())
                    {
                        IPropertyDescriptor propertyDescriptor = CreateSetPropertyDescriptor(propertyInfo);
                        ICodeObject valueObject = context.ParserContext.ParserService.ProcessValue(
                            attribute.Value,
                            new DefaultParserServiceContext(context.ParserContext.DependencyProvider, context.ParserContext.Errors)
                        );

                        if (valueObject != null)
                        {
                            propertyDescriptor.SetValue(valueObject);
                            codeObject.Properties.Add(propertyDescriptor);
                            boundProperies.Add(propertyName);
                        }
                    }
                }
            }

            if (defaultProperty != null && !boundProperies.Contains(defaultProperty.Name.ToLowerInvariant()))
            {
                IPropertyDescriptor propertyDescriptor = CreateSetPropertyDescriptor(defaultProperty);
                string defaultAttributeValue = extension.DefaultAttributes.FirstOrDefault();
                if (!String.IsNullOrEmpty(defaultAttributeValue))
                {
                    ICodeObject valueObject = context.ParserContext.ParserService.ProcessValue(
                        defaultAttributeValue,
                        new DefaultParserServiceContext(context.ParserContext.DependencyProvider, context.ParserContext.Errors)
                    );

                    if (valueObject != null)
                    {
                        propertyDescriptor.SetValue(valueObject);
                        codeObject.Properties.Add(propertyDescriptor);
                    }
                }
            }

            return true;
        }
    }
}
