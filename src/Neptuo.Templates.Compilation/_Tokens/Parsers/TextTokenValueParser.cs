using Neptuo.ComponentModel;
using Neptuo.ComponentModel.TextOffsets;
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
    public partial class TextTokenValueParser : ITextValueParser
    {
        private readonly IParserRegistry registry;

        public TextTokenValueParser(IParserRegistry registry)
        {
            Ensure.NotNull(registry, "registry");
            this.registry = registry;
        }

        public ICodeObject Parse(ISourceContent content, ITextValueParserContext context)
        {
            ICodeObject codeObject = null;

            TokenParser parser = CreateTokenParser();
            parser.OnParsedToken += (sender, e) => codeObject = GenerateToken(context, e.Token, content.GlobalSourceInfo);
            if (!parser.Parse(content.TextContent))
                codeObject = null;

            return codeObject;
        }

        private ICodeObject GenerateToken(ITextValueParserContext context, Token token, ILineInfo globalSourceInfo)
        {
            // Update line info according to global source info.
            token.SetLineInfo(
                globalSourceInfo.LineIndex + token.LineIndex, 
                globalSourceInfo.ColumnIndex + token.ColumnIndex, 
                globalSourceInfo.LineIndex + token.EndLineIndex, 
                globalSourceInfo.ColumnIndex + token.EndColumnIndex
            );

            // Build token.
            ICodeObject codeObject = registry.WithTokenBuilder().TryParse(new TokenBuilderContext(this, context, registry), token);
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
