using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.Parsers
{
    public interface IContentParser
    {
        bool Parse(string content, IContentParserContext context);
    }
}
