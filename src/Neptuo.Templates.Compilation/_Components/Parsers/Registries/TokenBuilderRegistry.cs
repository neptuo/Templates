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
    /// Registry for <see cref="ITokenValueBuilder"/> by prefix and name of token.
    /// </summary>
    public class TokenBuilderRegistry : ITokenValueBuilder
    {
        private readonly Dictionary<string, Dictionary<string, ITokenValueBuilder>> storage = new Dictionary<string, Dictionary<string, ITokenValueBuilder>>();
        private readonly INameNormalizer nameNormalizer;
        private FuncList<Token, ITokenValueBuilder> onSearchBuilder = new FuncList<Token, ITokenValueBuilder>(o => new NullBuilder());

        /// <summary>
        /// Creates new instance with <paramref name="nameNormalizer"/> for normalizing names.
        /// </summary>
        /// <param name="nameNormalizer">Normalizer for normlizing names.</param>
        public TokenBuilderRegistry(INameNormalizer nameNormalizer)
        {
            Guard.NotNull(nameNormalizer, "nameNormalizer");
            this.nameNormalizer = nameNormalizer;
        }

        /// <summary>
        /// Maps <paramref name="builder"/> to process tokens with <paramref name="prefix" /> and <paramref name="name" />.
        /// </summary>
        public TokenBuilderRegistry AddBuilder(string prefix, string name, ITokenValueBuilder builder)
        {
            Guard.NotNullOrEmpty(name, "name");
            Guard.NotNull(builder, "builder");

            prefix = nameNormalizer.PreparePrefix(prefix);
            name = nameNormalizer.PrepareName(name);

            Dictionary<string, ITokenValueBuilder> builders;
            if (!storage.TryGetValue(prefix, out builders))
                storage[prefix] = builders = new Dictionary<string, ITokenValueBuilder>();

            builders[name] = builder;
            return this;
        }

        /// <summary>
        /// Adds <paramref name="searchHandler"/> to be executed when builder was not found for token.
        /// (Last registered is executed the first).
        /// </summary>
        /// <param name="searchHandler">Builder provider method.</param>
        public TokenBuilderRegistry AddSearchHandler(Func<Token, ITokenValueBuilder> searchHandler)
        {
            Guard.NotNull(searchHandler, "searchHandler");
            onSearchBuilder.Add(searchHandler);
            return this;
        }

        public ICodeObject TryParse(ITokenValueBuilderContext context, Token token)
        {
            string prefix = nameNormalizer.PreparePrefix(token.Prefix);
            string name = nameNormalizer.PrepareName(token.Name);

            Dictionary<string, ITokenValueBuilder> builders;
            if (storage.TryGetValue(prefix, out builders))
            {
                ITokenValueBuilder builder;
                if (!builders.TryGetValue(name, out builder))
                    builder = onSearchBuilder.Execute(token);

                if (builder != null)
                    return builder.TryParse(context, token);
            }

            return null;
        }

        private class NullBuilder : ITokenValueBuilder
        {
            public ICodeObject TryParse(ITokenValueBuilderContext context, Token token)
            {
                context.AddError(token, String.Format("Unnable to process token '{0}'.", token.Fullname));
                return null;
            }
        }
    }
}
