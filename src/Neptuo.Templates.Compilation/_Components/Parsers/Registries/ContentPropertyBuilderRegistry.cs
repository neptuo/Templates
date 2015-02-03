﻿using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers.Normalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Registry for <see cref="IPropertyBuilder"/> and <see cref="IContentPropertyBuilder"/> by property type.
    /// </summary>
    public class ContentPropertyBuilderRegistry : IContentPropertyBuilder
    {
        private readonly Dictionary<Type, IContentPropertyBuilder> contentStorage = new Dictionary<Type, IContentPropertyBuilder>();
        private readonly Dictionary<Type, IPropertyBuilder> storage = new Dictionary<Type, IPropertyBuilder>();
        private Func<IPropertyInfo, IContentPropertyBuilder> onSearchContentBuilder = o => new NullBuilder();
        private Func<IPropertyInfo, IPropertyBuilder> onSearchBuilder = o => new NullBuilder();

        /// <summary>
        /// Maps <paramref name="builder"/> to process properties of type <paramref name="builder" />
        /// </summary>
        public ContentPropertyBuilderRegistry AddBuilder(Type propertyType, IContentPropertyBuilder builder)
        {
            Guard.NotNull(propertyType, "propertyType");
            Guard.NotNull(builder, "builder");

            contentStorage[propertyType] = builder;
            return this;
        }

        /// <summary>
        /// Maps <paramref name="builder"/> to process properties of type <paramref name="builder" />
        /// </summary>
        public ContentPropertyBuilderRegistry AddBuilder(Type propertyType, IPropertyBuilder builder)
        {
            Guard.NotNull(propertyType, "propertyType");
            Guard.NotNull(builder, "builder");

            storage[propertyType] = builder;
            return this;
        }

        /// <summary>
        /// Adds <paramref name="searchHandler"/> to be executed when builder was not found for property.
        /// (Last registered is executed the first).
        /// </summary>
        /// <param name="searchHandler">Builder provider method.</param>
        public ContentPropertyBuilderRegistry AddSearchHandler(Func<IPropertyInfo, IContentPropertyBuilder> searchHandler)
        {
            Guard.NotNull(searchHandler, "searchHandler");
            onSearchContentBuilder = searchHandler + onSearchContentBuilder;
            return this;
        }

        /// <summary>
        /// Adds <paramref name="searchHandler"/> to be executed when builder was not found for property.
        /// (Last registered is executed the first).
        /// </summary>
        /// <param name="searchHandler">Builder provider method.</param>
        public ContentPropertyBuilderRegistry AddSearchHandler(Func<IPropertyInfo, IPropertyBuilder> searchHandler)
        {
            Guard.NotNull(searchHandler, "searchHandler");
            onSearchBuilder = searchHandler + onSearchBuilder;
            return this;
        }

        public IEnumerable<ICodeProperty> TryParse(IContentPropertyBuilderContext context, IEnumerable<IXmlNode> content)
        {
            IContentPropertyBuilder builder;
            if (!contentStorage.TryGetValue(context.PropertyInfo.Type, out builder))
                builder = onSearchContentBuilder(context.PropertyInfo);

            if (builder != null)
                return builder.TryParse(context, content);

            return null;
        }

        public IEnumerable<ICodeProperty> TryParse(IPropertyBuilderContext context, ISourceContent value)
        {
            IPropertyBuilder builder;
            if (!storage.TryGetValue(context.PropertyInfo.Type, out builder))
                builder = onSearchBuilder(context.PropertyInfo);

            if (builder != null)
                return builder.TryParse(context, value);

            return null;
        }

        public class NullBuilder : IContentPropertyBuilder
        {
            public IEnumerable<ICodeProperty> TryParse(IContentPropertyBuilderContext context, IEnumerable<IXmlNode> content)
            {
                context.AddError(String.Format(
                    "Unnable to process property '{0}' of property type '{1}'.", 
                    context.PropertyInfo.Name, 
                    context.PropertyInfo.Type.FullName
                ));
                return null;
            }

            public IEnumerable<ICodeProperty> TryParse(IPropertyBuilderContext context, ISourceContent value)
            {
                context.AddError(String.Format(
                    "Unnable to process property '{0}' of property type '{1}'.",
                    context.PropertyInfo.Name,
                    context.PropertyInfo.Type.FullName
                ));
                return null;
            }
        }

    }
}