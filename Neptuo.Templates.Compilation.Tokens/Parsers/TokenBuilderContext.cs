using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Actuak implementaion of <see cref="ITokenBuilderContext"/>.
    /// </summary>
    public class TokenBuilderContext : ITokenBuilderContext
    {
        public IValueParserContext ParserContext { get; private set; }
        public IPropertyDescriptor Parent { get; private set; }
        public TokenValueParser Parser { get; private set; }
        public TokenValueParser.Helper Helper { get; private set; }
        public ITokenBuilderRegistry BuilderRegistry { get; private set; }

        public TokenBuilderContext(TokenValueParser parser, TokenValueParser.Helper helper)
        {
            Parser = parser;
            Helper = helper;
            ParserContext = helper.Context;
            Parent = helper.Context.PropertyDescriptor;
            BuilderRegistry = helper.BuilderRegistry;
        }
    }
}
