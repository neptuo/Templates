﻿using Neptuo.Models.Features;
using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers.Descriptors;
using Neptuo.Templates.Compilation.Parsers.Descriptors.Features;
using Neptuo.Templates.Compilation.Parsers.Normalization;
using Neptuo.Text.Tokens;
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
        protected abstract IComponentCodeObject CreateCodeObject(ITokenBuilderContext context, Token token);

        /// <summary>
        /// Should return component descriptor for <paramref name="token"/>.
        /// </summary>
        /// <param name="context">Builder context.</param>
        /// <param name="codeObject">Code object for <paramref name="token"/>.</param>
        /// <param name="token">Token to create component descriptor for.</param>
        /// <returns>Component descriptor for <paramref name="token"/>.</returns>
        protected abstract IComponentDescriptor GetComponentDescriptor(ITokenBuilderContext context, ICodeObject codeObject, Token token);
        
        public ICodeObject TryParse(ITokenBuilderContext context, Token extension)
        {
            IComponentCodeObject codeObject = CreateCodeObject(context, extension);
            IFieldCollectionCodeObject fields = codeObject.With<IFieldCollectionCodeObject>();
            if (fields != null && BindProperties(context, fields, extension))
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
        protected virtual bool BindProperties(ITokenBuilderContext context, IFieldCollectionCodeObject codeObject, Token token)
        {
            bool result = true;
            HashSet<string> boundProperies = new HashSet<string>();
            INameNormalizer nameNormalizer = context.Registry.WithPropertyNormalizer();
            IComponentDescriptor componentDefinition = GetComponentDescriptor(context, codeObject, token);
            IFieldDescriptor defaultField = componentDefinition.With<IDefaultFieldEnumerator>().FirstOrDefault();

            BindPropertiesContext<TokenAttribute> bindContext = new BindPropertiesContext<TokenAttribute>(componentDefinition, context.Registry.WithPropertyNormalizer());
            foreach (TokenAttribute attribute in token.Attributes)
            {
                string name = nameNormalizer.PrepareName(attribute.Name);

                IFieldDescriptor fieldDescriptor;
                if (bindContext.Fields.TryGetValue(name, out fieldDescriptor))
                {
                    IEnumerable<ICodeProperty> codeProperties = context.TryProcessProperty(fieldDescriptor, new DefaultSourceContent(attribute.Value, token));
                    if(codeProperties != null) 
                    {
                        foreach (ICodeProperty codeProperty in codeProperties)
                            codeObject.AddProperty(codeProperty);

                        bindContext.BoundProperties.Add(name);
                        continue;
                    }
                }

                context.AddError(token, String.Format("Unnable to bind attribute '{0}'.", attribute.Name));
                result = false;
            }

            if (defaultField != null && !boundProperies.Contains(nameNormalizer.PrepareName(defaultField.Name)))
            {
                string defaultAttributeValue = token.DefaultAttributes.FirstOrDefault();
                if (!String.IsNullOrEmpty(defaultAttributeValue))
                {
                    IEnumerable<ICodeProperty> codeProperties = context.TryProcessProperty(defaultField, new DefaultSourceContent(defaultAttributeValue, token));
                    if(codeProperties != null)
                    {
                        foreach (ICodeProperty codeProperty in codeProperties)
                            codeObject.AddProperty(codeProperty);

                        bindContext.BoundProperties.Add(nameNormalizer.PrepareName(defaultField.Name));
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