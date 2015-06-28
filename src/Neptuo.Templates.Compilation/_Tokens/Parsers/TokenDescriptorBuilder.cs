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
    /// Base implementation of <see cref="ITokenBuilder"/> that uses <see cref="IXComponentDescriptor"/> as target decriptor.
    /// </summary>
    public abstract class TokenDescriptorBuilder : ITokenBuilder
    {
        /// <summary>
        /// Should create code object for <paramref name="token"/>.
        /// </summary>
        /// <param name="context">Builder context.</param>
        /// <param name="token">Token to create code object for.</param>
        /// <returns>Code object for <paramref name="token"/>.</returns>
        protected abstract ICodeObject CreateCodeObject(ITokenBuilderContext context, Token token);

        /// <summary>
        /// Should return component descriptor for <paramref name="token"/>.
        /// </summary>
        /// <param name="context">Builder context.</param>
        /// <param name="codeObject">Code object for <paramref name="token"/>.</param>
        /// <param name="token">Token to create component descriptor for.</param>
        /// <returns>Component descriptor for <paramref name="token"/>.</returns>
        protected abstract IXComponentDescriptor GetComponentDescriptor(ITokenBuilderContext context, ICodeObject codeObject, Token token);
        
        public ICodeObject TryParse(ITokenBuilderContext context, Token extension)
        {
            ICodeObject codeObject = CreateCodeObject(context, extension);
            IPropertiesCodeObject propertiesCodeObject = codeObject as IPropertiesCodeObject;
            if (propertiesCodeObject != null && BindProperties(context, propertiesCodeObject, extension))
                return codeObject;

            return null;
        }

        /// <summary>
        /// Creates component descriptor for <paramref name="token"/> and tries to bind attributes from <paramref name="token"/> to <paramref name="codeObject"/>.
        /// </summary>
        /// <param name="context">Builder context.</param>
        /// <param name="codeObject">Code object to bind properties to.</param>
        /// <param name="token">Token to process attributes from.</param>
        /// <returns><c>true</c> if binding was successfull; otherwise <c>false</c>.</returns>
        protected virtual bool BindProperties(ITokenBuilderContext context, IPropertiesCodeObject codeObject, Token token)
        {
            bool result = true;
            HashSet<string> boundProperies = new HashSet<string>();
            INameNormalizer nameNormalizer = context.Registry.WithPropertyNormalizer();
            IXComponentDescriptor componentDefinition = GetComponentDescriptor(context, codeObject, token);
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
