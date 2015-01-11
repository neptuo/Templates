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
        private readonly ITokenBuilder builderFactory;

        public TokenValueParser(ITokenBuilder builderFactory)
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
            ICodeObject codeObject = builderFactory.TryParse(new TokenBuilderContext(this, context), token);
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
