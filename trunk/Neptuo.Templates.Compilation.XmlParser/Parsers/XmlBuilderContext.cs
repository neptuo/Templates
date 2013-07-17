using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class XmlBuilderContext : IBuilderContext
    {
        public IContentParserContext ParserContext { get; set; }
        public IPropertyDescriptor Parent { get; set; }
        public XmlContentParser Parser { get; set; }
        public XmlContentParser.Helper Helper { get; set; }
        public ILiteralBuilder LiteralBuilder { get; set; }
        public IComponentBuilder GenericContentBuilder { get; set; }
        public IBuilderRegistry BuilderRegistry { get; set; }

        public static XmlBuilderContext Create()
        {
            return new XmlBuilderContext();
        }

        public XmlBuilderContext SetParserContext(IContentParserContext parserContext)
        {
            ParserContext = parserContext;
            return this;
        }

        public XmlBuilderContext SetParent(IPropertyDescriptor parent)
        {
            Parent = parent;
            return this;
        }

        public XmlBuilderContext SetParser(XmlContentParser parser)
        {
            Parser = parser;
            return this;
        }

        public XmlBuilderContext SetHelper(XmlContentParser.Helper helper)
        {
            Helper = helper;
            SetParent(helper.Parent);
            SetParserContext(helper.Context);
            SetBuilderRegistry(helper.BuilderRegistry);
            return this;
        }

        public XmlBuilderContext SetLiteralBuilder(ILiteralBuilder literalBuilder)
        {
            LiteralBuilder = literalBuilder;
            return this;
        }

        public XmlBuilderContext SetGenericContent(IComponentBuilder genericContentBuilder)
        {
            GenericContentBuilder = genericContentBuilder;
            return this;
        }

        public XmlBuilderContext SetBuilderRegistry(IBuilderRegistry builderRegistry)
        {
            BuilderRegistry = builderRegistry;
            return this;
        }
    }
}
