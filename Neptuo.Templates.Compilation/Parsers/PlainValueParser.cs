using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class PlainValueParser : IValueParser
    {
        public bool Parse(string content, IValueParserContext context)
        {
            context.PropertyDescriptor.SetValue(new PlainValueCodeObject(content));
            return true;
        }
    }
}
