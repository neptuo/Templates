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
        public ITextValueParserContext ParserContext { get; private set; }
        public TextTokenValueParser Parser { get; private set; }
        public IParserProvider Registry { get; private set; }

        public TokenBuilderContext(TextTokenValueParser parser, ITextValueParserContext parserContext, IParserProvider registry)
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
