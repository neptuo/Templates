using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using System.Reflection;

namespace Neptuo.Templates.Compilation.Parsers
{
    public interface IValueParser
    {
        bool Parse(string content, IValueParserContext context);
    }
}
