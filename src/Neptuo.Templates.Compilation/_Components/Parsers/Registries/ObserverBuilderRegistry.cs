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
    /// Registry for <see cref="IXmlObserverBuilder"/> by prefix and name of token.
    /// </summary>
    public class ObserverBuilderRegistry : IXmlObserverBuilder
    {
        private readonly Dictionary<string, Dictionary<string, IXmlObserverBuilder>> storage = new Dictionary<string, Dictionary<string, IXmlObserverBuilder>>();
        private readonly INameNormalizer nameNormalizer;
        private FuncList<IXmlAttribute, IXmlObserverBuilder> onSearchBuilder = new FuncList<IXmlAttribute, IXmlObserverBuilder>(o => new NullBuilder());

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
        public ObserverBuilderRegistry AddBuilder(string prefix, string name, IXmlObserverBuilder builder)
        {
            Guard.NotNullOrEmpty(name, "name");
            Guard.NotNull(builder, "builder");

            prefix = nameNormalizer.PreparePrefix(prefix);
            name = nameNormalizer.PrepareName(name);

            Dictionary<string, IXmlObserverBuilder> builders;
            if (!storage.TryGetValue(prefix, out builders))
                storage[prefix] = builders = new Dictionary<string, IXmlObserverBuilder>();

            builders[name] = builder;
            return this;
        }

        /// <summary>
        /// Adds <paramref name="searchHandler"/> to be executed when builder was not found for observer.
        /// (Last registered is executed the first).
        /// </summary>
        /// <param name="searchHandler">Builder provider method.</param>
        public ObserverBuilderRegistry AddSearchHandler(Func<IXmlAttribute, IXmlObserverBuilder> searchHandler)
        {
            Guard.NotNull(searchHandler, "searchHandler");
            onSearchBuilder.Add(searchHandler);
            return this;
        }

        public bool TryParse(IXmlContentBuilderContext context, IObserversCodeObject codeObject, IXmlAttribute attribute)
        {
            string prefix = nameNormalizer.PreparePrefix(attribute.Prefix);
            string name = nameNormalizer.PrepareName(attribute.LocalName);

            Dictionary<string, IXmlObserverBuilder> builders;
            if (storage.TryGetValue(prefix, out builders))
            {
                IXmlObserverBuilder builder;
                if (!builders.TryGetValue(name, out builder))
                {
                    if(!builders.TryGetValue("*", out builder))
                        builder = onSearchBuilder.Execute(attribute);
                }

                if (builder != null)
                    return builder.TryParse(context, codeObject, attribute);
            }

            return false;
        }

        private class NullBuilder : IXmlObserverBuilder
        {
            public bool TryParse(IXmlContentBuilderContext context, IObserversCodeObject codeObject, IXmlAttribute attribute)
            {
                return false;
            }
        }
    }
}
