using Neptuo.Web.Framework.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.Parsers
{
    public class DefaultXmlBuilderContext : IXmlBuilderContext
    {
        public IContentParserContext ParserContext { get; set; }
        public IPropertyDescriptor Parent { get; set; }
        public XmlContentParser Parser { get; set; }
        public XmlContentParser.Helper Helper { get; set; }
        public XmlContentParser.LiteralTypeDescriptor LiteralTypeDescriptor { get; set; }
        public XmlContentParser.GenericContentTypeDescriptor GenericContentTypeDescriptor { get; set; }
        public IRegistrator Registrator { get; set; }

        public static DefaultXmlBuilderContext Create()
        {
            return new DefaultXmlBuilderContext();
        }

        public DefaultXmlBuilderContext SetParserContext(IContentParserContext parserContext)
        {
            ParserContext = parserContext;
            return this;
        }

        public DefaultXmlBuilderContext SetParent(IPropertyDescriptor parent)
        {
            Parent = parent;
            return this;
        }

        public DefaultXmlBuilderContext SetParser(XmlContentParser parser)
        {
            Parser = parser;
            return this;
        }

        public DefaultXmlBuilderContext SetHelper(XmlContentParser.Helper helper)
        {
            Helper = helper;
            return this;
        }

        public DefaultXmlBuilderContext SetLiteralTypeDescriptor(XmlContentParser.LiteralTypeDescriptor literalTypeDescriptor)
        {
            LiteralTypeDescriptor = literalTypeDescriptor;
            return this;
        }

        public DefaultXmlBuilderContext SetGenericContentTypeDescriptor(XmlContentParser.GenericContentTypeDescriptor genericContentTypeDescriptor)
        {
            GenericContentTypeDescriptor = genericContentTypeDescriptor;
            return this;
        }

        public DefaultXmlBuilderContext SetRegistrator(IRegistrator registrator)
        {
            Registrator = registrator;
            return this;
        }
    }
}
