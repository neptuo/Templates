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
        protected abstract IComponentCodeObject CreateCodeObject(ITokenBuilderContext context, Token extension);
        protected abstract IComponentDescriptor GetComponentDescriptor(ITokenBuilderContext context, IComponentCodeObject codeObject, Token extension);
        
        protected IPropertyBuilder PropertyFactory { get; private set; }

        public TokenDescriptorBuilder(IPropertyBuilder propertyFactory)
        {
            Guard.NotNull(propertyFactory, "propertyFactory");
            PropertyFactory = propertyFactory;
        }

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
            IComponentDescriptor componentDefinition = GetComponentDescriptor(context, codeObject, token);
            IPropertyInfo defaultProperty = componentDefinition.GetDefaultProperty();

            Dictionary<string, IPropertyInfo> properties = componentDefinition.GetProperties().ToDictionary(p => p.Name.ToLowerInvariant());
            HashSet<string> boundProperties = new HashSet<string>();
            foreach (TokenAttribute attribute in token.Attributes)
            {
                string name = attribute.Name.ToLowerInvariant();

                IPropertyInfo propertyInfo;
                if (properties.TryGetValue(name, out propertyInfo))
                {
                    IContentParserContext propertyContext = context.ParserContext.CreateContentContext(context.ParserContext.ParserService);
                    if (PropertyFactory.TryParse(propertyContext, codeObject, propertyInfo, new DefaultSourceContent(attribute.Value, token)))
                    {
                        boundProperties.Add(name);
                        continue;
                    }
                }

                context.AddError(token, String.Format("Unnable to bind attribute '{0}'.", attribute.Name));
                result = false;
            }

            if (defaultProperty != null && !boundProperies.Contains(defaultProperty.Name.ToLowerInvariant()))
            {
                string defaultAttributeValue = token.DefaultAttributes.FirstOrDefault();
                if (!String.IsNullOrEmpty(defaultAttributeValue))
                {
                    IContentParserContext propertyContext = context.ParserContext.CreateContentContext(context.ParserContext.ParserService);
                    if (PropertyFactory.TryParse(propertyContext, codeObject, defaultProperty, new DefaultSourceContent(defaultAttributeValue, token)))
                    {
                        boundProperties.Add(defaultProperty.Name.ToLowerInvariant());
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
