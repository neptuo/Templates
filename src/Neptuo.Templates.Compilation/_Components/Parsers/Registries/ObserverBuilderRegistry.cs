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
    /// Registry for <see cref="IObserverBuilder"/> by prefix and name of token.
    /// </summary>
    public class ObserverBuilderRegistry : IObserverBuilder
    {
        private readonly Dictionary<string, Dictionary<string, IObserverBuilder>> storage = new Dictionary<string, Dictionary<string, IObserverBuilder>>();
        private readonly INameNormalizer nameNormalizer;
        private Func<IXmlAttribute, IObserverBuilder> onSearchBuilder = o => new NullBuilder();

        /// <summary>
        /// Creates new instance with <paramref name="nameNormalizer"/> for normalizing names.
        /// </summary>
        /// <param name="nameNormalizer">Normalizer for normlizing names.</param>
        public ObserverBuilderRegistry(INameNormalizer nameNormalizer)
        {
            Guard.NotNull(nameNormalizer, "nameNormalizer");
            this.nameNormalizer = nameNormalizer;
        }

        /// <summary>
        /// Maps <paramref name="builder"/> to process observers with <paramref name="prefix" /> and <paramref name="name" />.
        /// </summary>
        public ObserverBuilderRegistry AddBuilder(string prefix, string name, IObserverBuilder builder)
        {
            Guard.NotNullOrEmpty(name, "name");
            Guard.NotNull(builder, "builder");

            prefix = nameNormalizer.PreparePrefix(prefix);
            name = nameNormalizer.PrepareName(name);

            Dictionary<string, IObserverBuilder> builders;
            if (!storage.TryGetValue(prefix, out builders))
                storage[prefix] = builders = new Dictionary<string, IObserverBuilder>();

            builders[name] = builder;
            return this;
        }

        /// <summary>
        /// Adds <paramref name="searchHandler"/> to be executed when builder was not found for observer.
        /// (Last registered is executed the first).
        /// </summary>
        /// <param name="searchHandler">Builder provider method.</param>
        public ObserverBuilderRegistry AddSearchHandler(Func<IXmlAttribute, IObserverBuilder> searchHandler)
        {
            Guard.NotNull(searchHandler, "searchHandler");
            onSearchBuilder = searchHandler + onSearchBuilder;
            return this;
        }
        
        public bool TryParse(IContentBuilderContext context, IComponentCodeObject codeObject, IXmlAttribute attribute)
        {
            string prefix = nameNormalizer.PreparePrefix(attribute.Prefix);
            string name = nameNormalizer.PrepareName(attribute.LocalName);

            Dictionary<string, IObserverBuilder> builders;
            if (storage.TryGetValue(prefix, out builders))
            {
                IObserverBuilder builder;
                if (!builders.TryGetValue(name, out builder))
                    builder = onSearchBuilder(attribute);

                if (builder != null)
                    return builder.TryParse(context, codeObject, attribute);
            }

            return false;
        }

        private class NullBuilder : IObserverBuilder
        {
            public bool TryParse(IContentBuilderContext context, IComponentCodeObject codeObject, IXmlAttribute attribute)
            {
                return false;
            }
        }
    }
}
