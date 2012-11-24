using Neptuo.Web.Framework.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.Parsers
{
    public interface IXmlBuilderContext
    {
        IContentParserContext ParserContext { get; }
        IPropertyDescriptor Parent { get; }
        XmlContentParser Parser { get; }
        XmlContentParser.Helper Helper { get; }
        XmlContentParser.LiteralTypeDescriptor LiteralTypeDescriptor { get; }
        XmlContentParser.GenericContentTypeDescriptor GenericContentTypeDescriptor { get; }
        IRegistrator Registrator { get; }
    }
}
