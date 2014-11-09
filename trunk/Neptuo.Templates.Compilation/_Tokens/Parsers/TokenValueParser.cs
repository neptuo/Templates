using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers;
using Neptuo.Tokens;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Token value parser.
    /// </summary>
    public partial class TokenValueParser : IValueParser
    {
        private readonly ITokenBuilderFactory builderFactory;

        public TokenValueParser(ITokenBuilderFactory builderFactory)
        {
            Guard.NotNull(builderFactory, "builderFactory");
            this.builderFactory = builderFactory;
        }

        public ICodeObject Parse(ISourceContent content, IValueParserContext context)
        {
            ICodeObject codeObject = null;

            TokenParser parser = CreateTokenParser();
            parser.OnParsedToken += (sender, e) => codeObject = GenerateToken(context, e.Token);
            parser.Parse(content.TextContent);

            return codeObject;
        }

        private ICodeObject GenerateToken(IValueParserContext context, Token token)
        {
            ITokenBuilder builder = builderFactory.CreateBuilder(token.Prefix, token.Name);
            if (builder == null)
                return null;

            ICodeObject codeObject = builder.Parse(new TokenBuilderContext(this, context, builderFactory), token);
            return codeObject;
        }

        private TokenParser CreateTokenParser()
        {
            TokenParser parser = new TokenParser();
            parser.Configuration.AllowAttributes = true;
            parser.Configuration.AllowDefaultAttributes = true;
            parser.Configuration.AllowMultipleTokens = false;
            parser.Configuration.AllowTextContent = false;
            return parser;
        }
    }
}
