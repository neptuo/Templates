using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers.Normalization;
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
        protected abstract IComponentCodeObject CreateCodeObject(ITokenBuilderContext context, Token extension);
        protected abstract IComponentDescriptor GetComponentDescriptor(ITokenBuilderContext context, IComponentCodeObject codeObject, Token extension);
        
        public ICodeObject TryParse(ITokenBuilderContext context, Token extension)
        {
            IComponentCodeObject codeObject = CreateCodeObject(context, extension);
            if (codeObject != null && BindProperties(context, codeObject, extension))
                return codeObject;

            return null;
        }

        protected virtual bool BindProperties(ITokenBuilderContext context, IComponentCodeObject codeObject, Token token)
        {
            bool result = true;
            HashSet<string> boundProperies = new HashSet<string>();
            INameNormalizer nameNormalizer = context.Registry.WithPropertyNormalizer();
            IComponentDescriptor componentDefinition = GetComponentDescriptor(context, codeObject, token);
            IPropertyInfo defaultProperty = componentDefinition.GetDefaultProperty();

            BindPropertiesContext<TokenAttribute> bindContext = new BindPropertiesContext<TokenAttribute>(componentDefinition, context.Registry.WithPropertyNormalizer());
            foreach (TokenAttribute attribute in token.Attributes)
            {
                string name = nameNormalizer.PrepareName(attribute.Name);

                IPropertyInfo propertyInfo;
                if (bindContext.Properties.TryGetValue(name, out propertyInfo))
                {
                    IEnumerable<ICodeProperty> codeProperties = context.TryProcessProperty(propertyInfo, new DefaultSourceContent(attribute.Value, token));
                    if(codeProperties != null) 
                    {
                        codeObject.Properties.AddRange(codeProperties);
                        bindContext.BoundProperties.Add(name);
                        continue;
                    }
                }

                context.AddError(token, String.Format("Unnable to bind attribute '{0}'.", attribute.Name));
                result = false;
            }

            if (defaultProperty != null && !boundProperies.Contains(nameNormalizer.PrepareName(defaultProperty.Name)))
            {
                string defaultAttributeValue = token.DefaultAttributes.FirstOrDefault();
                if (!String.IsNullOrEmpty(defaultAttributeValue))
                {
                    IEnumerable<ICodeProperty> codeProperties = context.TryProcessProperty(defaultProperty, new DefaultSourceContent(defaultAttributeValue, token));
                    if(codeProperties != null)
                    {
                        codeObject.Properties.AddRange(codeProperties);
                        bindContext.BoundProperties.Add(nameNormalizer.PrepareName(defaultProperty.Name));
                    }
                    else
                    {
                        context.AddError(token, "Unnable to bind default attribute.");
                        result = false;
                    }
                }
            }

            return result;
        }
    }
}
