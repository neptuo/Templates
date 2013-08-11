using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class XmlContentBuilderContext : IContentBuilderContext
    {
        public IContentParserContext ParserContext { get { return Helper.Context; } }
        public IPropertyDescriptor Parent { get { return Helper.Parent; } }
        public XmlContentParser Parser { get; set; }
        public XmlContentParser.Helper Helper { get; set; }
        public IContentBuilderRegistry BuilderRegistry { get; set; }

        public XmlContentBuilderContext(XmlContentParser parser, XmlContentParser.Helper helper, IContentBuilderRegistry builderRegistry)
        {
            Parser = parser;
            Helper = helper;
            BuilderRegistry = builderRegistry;
        }
    }
}
