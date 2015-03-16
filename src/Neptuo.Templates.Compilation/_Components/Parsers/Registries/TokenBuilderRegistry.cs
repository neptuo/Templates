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
    /// Registry for <see cref="ITokenBuilder"/> by prefix and name of token.
    /// </summary>
    public class TokenBuilderRegistry : ITokenBuilder
    {
        private readonly Dictionary<string, Dictionary<string, ITokenBuilder>> storage = new Dictionary<string, Dictionary<string, ITokenBuilder>>();
        private readonly INameNormalizer nameNormalizer;
        private FuncList<Token, ITokenBuilder> onSearchBuilder = new FuncList<Token, ITokenBuilder>(o => new NullBuilder());

        /// <summary>
        /// Creates new instance with <paramref name="nameNormalizer"/> for normalizing names.
        /// </summary>
        /// <param name="nameNormalizer">Normalizer for normlizing names.</param>
        public TokenBuilderRegistry(INameNormalizer nameNormalizer)
        {
            Ensure.NotNull(nameNormalizer, "nameNormalizer");
            this.nameNormalizer = nameNormalizer;
        }

        /// <summary>
        /// Maps <paramref name="builder"/> to process tokens with <paramref name="prefix" /> and <paramref name="name" />.
        /// </summary>
        public TokenBuilderRegistry AddBuilder(string prefix, string name, ITokenBuilder builder)
        {
            Ensure.NotNullOrEmpty(name, "name");
            Ensure.NotNull(builder, "builder");

            prefix = nameNormalizer.PreparePrefix(prefix);
            name = nameNormalizer.PrepareName(name);

            Dictionary<string, ITokenBuilder> builders;
            if (!storage.TryGetValue(prefix, out builders))
                storage[prefix] = builders = new Dictionary<string, ITokenBuilder>();

            builders[name] = builder;
            return this;
        }

        /// <summary>
        /// Adds <paramref name="searchHandler"/> to be executed when builder was not found for token.
        /// (Last registered is executed the first).
        /// </summary>
        /// <param name="searchHandler">Builder provider method.</param>
        public TokenBuilderRegistry AddSearchHandler(Func<Token, ITokenBuilder> searchHandler)
        {
            Ensure.NotNull(searchHandler, "searchHandler");
            onSearchBuilder.Add(searchHandler);
            return this;
        }

        public ICodeObject TryParse(ITokenBuilderContext context, Token token)
        {
            string prefix = nameNormalizer.PreparePrefix(token.Prefix);
            string name = nameNormalizer.PrepareName(token.Name);

            Dictionary<string, ITokenBuilder> builders;
            if (storage.TryGetValue(prefix, out builders))
            {
                ITokenBuilder builder;
                if (!builders.TryGetValue(name, out builder))
                    builder = onSearchBuilder.Execute(token);

                if (builder != null)
                    return builder.TryParse(context, token);
            }

            return null;
        }

        private class NullBuilder : ITokenBuilder
        {
            public ICodeObject TryParse(ITokenBuilderContext context, Token token)
            {
                context.AddError(token, String.Format("Unnable to process token '{0}'.", token.Fullname));
                return null;
            }
        }
    }
}
