using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Base implementaion of <see cref="ITokenBuilderContext"/>.
    /// </summary>
    public class TokenBuilderContext : ITokenBuilderContext
    {
        public IValueParserContext ParserContext { get; private set; }
        public TokenValueParser Parser { get; private set; }
        public ITokenBuilderFactory BuilderFactory { get; private set; }

        public TokenBuilderContext(TokenValueParser parser, IValueParserContext parserContext, ITokenBuilderFactory builderFactory)
        {
            Guard.NotNull(parser, "parser");
            Guard.NotNull(parserContext, "parserContext");
            Guard.NotNull(builderFactory, "builderFactory");
            Parser = parser;
            ParserContext = parserContext;
            BuilderFactory = builderFactory;
        }
    }
}
