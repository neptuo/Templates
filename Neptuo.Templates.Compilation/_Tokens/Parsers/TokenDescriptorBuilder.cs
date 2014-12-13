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
        protected abstract ITokenDescriptor GetTokenDefinition(ITokenBuilderContext context, IValueExtensionCodeObject codeObject, Token extension);

        protected abstract IPropertyDescriptor CreateSetPropertyDescriptor(IPropertyInfo propertyInfo);
        protected abstract IPropertyDescriptor CreateListAddPropertyDescriptor(IPropertyInfo propertyInfo);

        public ICodeObject TryParse(ITokenBuilderContext context, Token extension)
        {
            IValueExtensionCodeObject codeObject = CreateCodeObject(context, extension);
            if (codeObject != null && BindProperties(context, codeObject, extension))
                return codeObject;

            return null;
        }

        protected virtual bool BindProperties(ITokenBuilderContext context, IValueExtensionCodeObject codeObject, Token token)
        {
            HashSet<string> boundProperies = new HashSet<string>();
            ITokenDescriptor extensionDefinition = GetTokenDefinition(context, codeObject, token);
            IPropertyInfo defaultProperty = extensionDefinition.GetDefaultProperty();

            foreach (IPropertyInfo propertyInfo in extensionDefinition.GetProperties())
            {
                string propertyName = propertyInfo.Name.ToLowerInvariant();
                foreach (TokenAttribute attribute in token.Attributes)
                {
                    if (propertyName == attribute.Name.ToLowerInvariant())
                    {
                        IPropertyDescriptor propertyDescriptor = CreateSetPropertyDescriptor(propertyInfo);
                        ICodeObject valueObject = context.ProcessValue(attribute.GetValue());

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
                string defaultAttributeValue = token.DefaultAttributes.FirstOrDefault();
                if (!String.IsNullOrEmpty(defaultAttributeValue))
                {
                    ICodeObject valueObject = context.ProcessValue(new DefaultSourceContent(defaultAttributeValue, token));
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
