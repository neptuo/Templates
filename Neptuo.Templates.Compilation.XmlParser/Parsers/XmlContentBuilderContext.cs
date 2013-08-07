using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class XmlContentBuilderContext : IContentBuilderContext
    {
        public IContentParserContext ParserContext { get; set; }
        public IPropertyDescriptor Parent { get; set; }
        public XmlContentParser Parser { get; set; }
        public XmlContentParser.Helper Helper { get; set; }
        public IContentBuilderRegistry BuilderRegistry { get; set; }

        public static XmlContentBuilderContext Create()
        {
            return new XmlContentBuilderContext();
        }

        public XmlContentBuilderContext SetParserContext(IContentParserContext parserContext)
        {
            ParserContext = parserContext;
            return this;
        }

        public XmlContentBuilderContext SetParent(IPropertyDescriptor parent)
        {
            Parent = parent;
            return this;
        }

        public XmlContentBuilderContext SetParser(XmlContentParser parser)
        {
            Parser = parser;
            return this;
        }

        public XmlContentBuilderContext SetHelper(XmlContentParser.Helper helper)
        {
            Helper = helper;
            SetParent(helper.Parent);
            SetParserContext(helper.Context);
            SetBuilderRegistry(helper.BuilderRegistry);
            return this;
        }

        public XmlContentBuilderContext SetBuilderRegistry(IContentBuilderRegistry builderRegistry)
        {
            BuilderRegistry = builderRegistry;
            return this;
        }
    }
}
