using Neptuo.Templates.Compilation.CodeObjects;
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
        private readonly FuncList<IPropertyInfo, IContentPropertyBuilder> onSearchContentBuilder = new FuncList<IPropertyInfo, IContentPropertyBuilder>(o => new NullBuilder());
        private readonly FuncList<IPropertyInfo, IPropertyBuilder> onSearchBuilder = new FuncList<IPropertyInfo, IPropertyBuilder>(o => new NullBuilder());

        /// <summary>
        /// Maps <paramref name="builder"/> to process properties of type <paramref name="builder" />
        /// </summary>
        public ContentPropertyBuilderRegistry AddBuilder(Type propertyType, IContentPropertyBuilder builder)
        {
            Ensure.NotNull(propertyType, "propertyType");
            Ensure.NotNull(builder, "builder");

            contentStorage[propertyType] = builder;
            return this;
        }

        /// <summary>
        /// Maps <paramref name="builder"/> to process properties of type <paramref name="builder" />
        /// </summary>
        public ContentPropertyBuilderRegistry AddBuilder(Type propertyType, IPropertyBuilder builder)
        {
            Ensure.NotNull(propertyType, "propertyType");
            Ensure.NotNull(builder, "builder");

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
            Ensure.NotNull(searchHandler, "searchHandler");
            onSearchContentBuilder.Add(searchHandler);
            onSearchBuilder.Add(searchHandler);
            return this;
        }

        /// <summary>
        /// Adds <paramref name="searchHandler"/> to be executed when builder was not found for property.
        /// (Last registered is executed the first).
        /// </summary>
        /// <param name="searchHandler">Builder provider method.</param>
        public ContentPropertyBuilderRegistry AddSearchHandler(Func<IPropertyInfo, IPropertyBuilder> searchHandler)
        {
            Ensure.NotNull(searchHandler, "searchHandler");
            onSearchBuilder.Add(searchHandler);
            return this;
        }

        public IEnumerable<ICodeProperty> TryParse(IContentPropertyBuilderContext context, IEnumerable<IXmlNode> content)
        {
            IContentPropertyBuilder builder;
            if (!contentStorage.TryGetValue(context.PropertyInfo.Type, out builder))
                builder = onSearchContentBuilder.Execute(context.PropertyInfo);

            if (builder != null)
                return builder.TryParse(context, content);

            return null;
        }

        public IEnumerable<ICodeProperty> TryParse(IPropertyBuilderContext context, ISourceContent value)
        {
            IPropertyBuilder builder;
            if (!storage.TryGetValue(context.PropertyInfo.Type, out builder))
                builder = onSearchBuilder.Execute(context.PropertyInfo);

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