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
    /// Registry for <see cref="IXmlContentBuilder"/> by prefix and name of element.
    /// </summary>
    public class ContentBuilderRegistry : IXmlContentBuilder
    {
        private readonly Dictionary<string, Dictionary<string, IXmlContentBuilder>> storage = new Dictionary<string, Dictionary<string, IXmlContentBuilder>>();
        private readonly INameNormalizer nameNormalizer;
        private readonly FuncList<IXmlElement, IXmlContentBuilder> onSearchBuilder = new FuncList<IXmlElement, IXmlContentBuilder>(o => new NullBuilder());

        /// <summary>
        /// Creates new instance with <paramref name="nameNormalizer"/> for normalizing names.
        /// </summary>
        /// <param name="nameNormalizer">Normalizer for normlizing names.</param>
        public ContentBuilderRegistry(INameNormalizer nameNormalizer)
        {
            Guard.NotNull(nameNormalizer, "nameNormalizer");
            this.nameNormalizer = nameNormalizer;
        }

        /// <summary>
        /// Maps <paramref name="builder"/> to process elements with <paramref name="prefix" /> and <paramref name="name" />.
        /// </summary>
        public ContentBuilderRegistry AddBuilder(string prefix, string name, IXmlContentBuilder builder)
        {
            Guard.NotNullOrEmpty(name, "name");
            Guard.NotNull(builder, "builder");

            prefix = nameNormalizer.PreparePrefix(prefix);
            name = nameNormalizer.PrepareName(name);

            Dictionary<string, IXmlContentBuilder> builders;
            if (!storage.TryGetValue(prefix, out builders))
                storage[prefix] = builders = new Dictionary<string, IXmlContentBuilder>();

            builders[name] = builder;
            return this;
        }

        /// <summary>
        /// Adds <paramref name="searchHandler"/> to be executed when builder was not found for element.
        /// (Last registered is executed the first).
        /// </summary>
        /// <param name="searchHandler">Builder provider method.</param>
        public ContentBuilderRegistry AddSearchHandler(Func<IXmlElement, IXmlContentBuilder> searchHandler)
        {
            Guard.NotNull(searchHandler, "searchHandler");
            onSearchBuilder.Add(searchHandler);
            return this;
        }

        public IEnumerable<ICodeObject> TryParse(IXmlContentBuilderContext context, IXmlElement element)
        {
            string prefix = nameNormalizer.PreparePrefix(element.Prefix);
            string name = nameNormalizer.PrepareName(element.LocalName);

            Dictionary<string, IXmlContentBuilder> builders;
            if (storage.TryGetValue(prefix, out builders))
            {
                IXmlContentBuilder builder;
                if (!builders.TryGetValue(name, out builder))
                    builder = onSearchBuilder.Execute(element);

                if (builder != null)
                    return builder.TryParse(context, element);
            }

            return null;
        }

        private class NullBuilder : IXmlContentBuilder
        {
            public IEnumerable<ICodeObject> TryParse(IXmlContentBuilderContext context, IXmlElement element)
            {
                context.AddError(element, String.Format("Unnable to process element '{0}'.", element.Name));
                return null;
            }
        }
    }
}
