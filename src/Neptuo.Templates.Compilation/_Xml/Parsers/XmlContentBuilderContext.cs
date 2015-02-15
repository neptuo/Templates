using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Base implementation of <see cref="IXmlContentBuilderContext"/>.
    /// </summary>
    public class XmlContentBuilderContext : IXmlContentBuilderContext
    {
        public IContentParserContext ParserContext { get; private set; }
        public Dictionary<string, object> CustomValues { get; private set; }
        public XmlContentParser Parser { get; private set; }
        public IParserRegistry Registry { get; private set; }

        public XmlContentBuilderContext(IXmlContentBuilderContext context)
            : this(context.ParserContext, context.Parser, context.Registry)
        { }

        public XmlContentBuilderContext(IContentParserContext parserContext, XmlContentParser parser, IParserRegistry registry)
        {
            Guard.NotNull(parserContext, "parserContext");
            Guard.NotNull(parser, "parser");
            Guard.NotNull(registry, "registry");
            ParserContext = parserContext;
            CustomValues = new Dictionary<string, object>();
            Parser = parser;
            Registry = registry;
        }
    }
}
