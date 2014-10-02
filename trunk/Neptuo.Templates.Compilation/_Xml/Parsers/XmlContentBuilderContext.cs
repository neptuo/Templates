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
        public IContentParserContext ParserContext { get { return Helper.Context; } }
        public XmlContentParser Parser { get; set; }
        public XmlContentParser.Helper Helper { get; set; }
        public IContentBuilderRegistry BuilderRegistry { get; set; }

        public XmlContentBuilderContext(XmlContentParser parser, XmlContentParser.Helper helper, IContentBuilderRegistry builderRegistry)
        {
            Guard.NotNull(parser, "parser");
            Guard.NotNull(helper, "helper");
            Guard.NotNull(builderRegistry, "builderRegistry");
            Parser = parser;
            Helper = helper;
            BuilderRegistry = builderRegistry;
        }
    }
}
