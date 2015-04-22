using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Base implementation of <see cref="IContentBuilderContext"/>.
    /// </summary>
    public class XmlContentBuilderContext : IContentBuilderContext
    {
        public ITextContentParserContext ParserContext { get; private set; }
        public Dictionary<string, object> CustomValues { get; private set; }
        public XmlContentParser Parser { get; private set; }
        public IParserRegistry Registry { get; private set; }

        public XmlContentBuilderContext(IContentBuilderContext context)
            : this(context.ParserContext, context.Parser, context.Registry)
        { }

        public XmlContentBuilderContext(ITextContentParserContext parserContext, XmlContentParser parser, IParserRegistry registry)
        {
            Ensure.NotNull(parserContext, "parserContext");
            Ensure.NotNull(parser, "parser");
            Ensure.NotNull(registry, "registry");
            ParserContext = parserContext;
            CustomValues = new Dictionary<string, object>();
            Parser = parser;
            Registry = registry;
        }
    }
}
