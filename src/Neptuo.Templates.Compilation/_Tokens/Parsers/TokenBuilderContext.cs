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
        public IParserRegistry Registry { get; private set; }

        public TokenBuilderContext(TokenValueParser parser, IValueParserContext parserContext, IParserRegistry registry)
        {
            Ensure.NotNull(parser, "parser");
            Ensure.NotNull(parserContext, "parserContext");
            Ensure.NotNull(registry, "registry");
            Parser = parser;
            ParserContext = parserContext;
            Registry = registry;
        }
    }
}
