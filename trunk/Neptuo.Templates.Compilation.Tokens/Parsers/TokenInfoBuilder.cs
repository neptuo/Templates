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
    public abstract class TokenInfoBuilder : ITokenBuilder
    {
        protected abstract IValueExtensionCodeObject CreateCodeObject(ITokenBuilderContext context, Token extension);
        protected abstract ITokenDescriptor GetExtensionDefinition(ITokenBuilderContext context, IValueExtensionCodeObject codeObject, Token extension);

        protected abstract IPropertyDescriptor CreateSetPropertyDescriptor(IPropertyInfo propertyInfo);
        protected abstract IPropertyDescriptor CreateListAddPropertyDescriptor(IPropertyInfo propertyInfo);

        public bool Parse(ITokenBuilderContext context, Token extension)
        {
            IValueExtensionCodeObject codeObject = CreateCodeObject(context, extension);
            if (codeObject == null)
                return false;

            context.Parent.SetValue(codeObject);
            return BindProperties(context, codeObject, extension);
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
