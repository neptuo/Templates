using Neptuo.Templates.Compilation.Parsers.Syntax.Nodes;
using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax
{
    /// <summary>
    /// Common extensions for <see cref="SyntaxParserService"/>.
    /// </summary>
    public static class _SyntaxParserServiceExtensions
    {
        /// <summary>
        /// Adds <see cref="AngleTokenBuilder"/> to the value tokenizer of <paramref name="service"/>.
        /// If <paramref name="isIncludedInValueTokenizer"/> is <c>true</c> angle token builder is also added to the value tokenizer.
        /// </summary>
        /// <param name="service">The service to add tokenizer to.</param>
        /// <param name="isIncludedInValueTokenizer">Whether to add angle token builder as value tokenizer.</param>
        /// <returns><paramref name="service"/>.</returns>
        public static SyntaxParserService AddAngleTokenBuilder(this SyntaxParserService service, bool isIncludedInValueTokenizer = false)
        {
            Ensure.NotNull(service, "service");
            service.ContentTokenizer.Add(new AngleTokenBuilder());

            if (isIncludedInValueTokenizer)
                service.ValueTokenizer.Add(new AngleTokenBuilder());

            return service;
        }

        /// <summary>
        /// Adds <see cref="CurlyTokenBuilder"/> to the value tokenizer of <paramref name="service"/>.
        /// If <paramref name="isIncludedInContentTokenizer"/> is <c>true</c> curly token builder is also added to the content tokenizer.
        /// </summary>
        /// <param name="service">The service to add tokenizer to.</param>
        /// <param name="isIncludedInContentTokenizer">Whether to add curly token builder as content tokenizer.</param>
        /// <returns><paramref name="service"/>.</returns>
        public static SyntaxParserService AddCurlyTokenBuilder(this SyntaxParserService service, bool isIncludedInContentTokenizer = true)
        {
            Ensure.NotNull(service, "service");
            service.ContentTokenizer.Add(new CurlyTokenBuilder());

            if (isIncludedInContentTokenizer)
                service.ValueTokenizer.Add(new CurlyTokenBuilder());

            return service;
        }

        /// <summary>
        /// Adds <see cref="LiteralTokenBuilder"/> as content and value tokenzer of <paramref name="service"/>.
        /// </summary>
        /// <param name="service">The service to add tokenizer to.</param>
        /// <returns><paramref name="service"/>.</returns>
        public static SyntaxParserService AddLiteralTokenBuilder(this SyntaxParserService service)
        {
            Ensure.NotNull(service, "service");
            service.ContentTokenizer.Add(new LiteralTokenBuilder());
            service.ContentTokenizer.Add(new LiteralTokenBuilder());
            return service;
        }


        /// <summary>
        /// Adds <paramref name="builder"/> to build syntax nodes of token <paramref name="type"/>.
        /// </summary>
        /// <param name="service">The service to add node builder to.</param>
        /// <param name="type">The type of token to be processed by <paramref name="builder"/>.</param>
        /// <param name="builder">The syntax node builder to add.</param>
        /// <returns><paramref name="service"/>.</returns>
        public static SyntaxParserService AddSyntaxNodeBuilder(this SyntaxParserService service, TokenType type, ISyntaxNodeBuilder builder)
        {
            Ensure.NotNull(service, "service");
            service.SyntaxBuilders.Add(type, builder);
            return service;
        }
    }
}
