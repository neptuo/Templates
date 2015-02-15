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
    /// Registry for <see cref="IPropertyBuilder"/> and <see cref="IXmlContentPropertyBuilder"/> by property type.
    /// </summary>
    public class ContentPropertyBuilderRegistry : IXmlContentPropertyBuilder
    {
        private readonly Dictionary<Type, IXmlContentPropertyBuilder> contentStorage = new Dictionary<Type, IXmlContentPropertyBuilder>();
        private readonly Dictionary<Type, IPropertyBuilder> storage = new Dictionary<Type, IPropertyBuilder>();
        private readonly FuncList<IPropertyInfo, IXmlContentPropertyBuilder> onSearchContentBuilder = new FuncList<IPropertyInfo, IXmlContentPropertyBuilder>(o => new NullBuilder());
        private readonly FuncList<IPropertyInfo, IPropertyBuilder> onSearchBuilder = new FuncList<IPropertyInfo, IPropertyBuilder>(o => new NullBuilder());

        /// <summary>
        /// Maps <paramref name="builder"/> to process properties of type <paramref name="builder" />
        /// </summary>
        public ContentPropertyBuilderRegistry AddBuilder(Type propertyType, IXmlContentPropertyBuilder builder)
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
        public ContentPropertyBuilderRegistry AddSearchHandler(Func<IPropertyInfo, IXmlContentPropertyBuilder> searchHandler)
        {
            Guard.NotNull(searchHandler, "searchHandler");
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
            Guard.NotNull(searchHandler, "searchHandler");
            onSearchBuilder.Add(searchHandler);
            return this;
        }

        public IEnumerable<ICodeProperty> TryParse(IXmlContentPropertyBuilderContext context, IEnumerable<IXmlNode> content)
        {
            IXmlContentPropertyBuilder builder;
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

        public class NullBuilder : IXmlContentPropertyBuilder
        {
            public IEnumerable<ICodeProperty> TryParse(IXmlContentPropertyBuilderContext context, IEnumerable<IXmlNode> content)
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