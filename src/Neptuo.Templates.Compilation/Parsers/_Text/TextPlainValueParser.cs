using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Parses any value as <see cref="IPlainValueCodeObject"/>.
    /// </summary>
    public class TextPlainValueParser : ITextValueParser
    {
        public ICodeObject Parse(ISourceContent content, ITextValueParserContext context)
        {
            return new PlainValueCodeObject(content.TextContent);
        }
    }
}
