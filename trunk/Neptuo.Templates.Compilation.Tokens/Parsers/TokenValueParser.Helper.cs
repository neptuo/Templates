using Neptuo.Templates.Compilation.Parsers;
using Neptuo.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Helper for parsing markup extensions.
    /// </summary>
    partial class TokenValueParser
    {
        public class Helper
        {
            public IValueParserContext Context { get; protected set; }
            public ITokenBuilderFactory BuilderFactory { get; protected set; }
            public TokenParser Parser { get; protected set; }

            public Helper(IValueParserContext context, ITokenBuilderFactory builderFactory)
            {
                Guard.NotNull(context, "context");
                Guard.NotNull(builderFactory, "builderFactory");
                Context = context;
                BuilderFactory = builderFactory;
                Parser = new TokenParser();
                Parser.Configuration.AllowAttributes = true;
                Parser.Configuration.AllowDefaultAttribute = true;
                Parser.Configuration.AllowMultipleTokens = false;
                Parser.Configuration.AllowTextContent = false;
            }
        }
    }
}
