using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public interface IMarkupExtensionBuilderContext
    {
        IValueParserContext ParserContext { get; }
        IPropertyDescriptor Parent { get; }
        MarkupExtensionValueParser Parser { get; }
        MarkupExtensionValueParser.Helper Helper { get; }
        IMarkupExtensionBuilderRegistry BuilderRegistry { get; }
    }
}
