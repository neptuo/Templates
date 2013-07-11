using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Parser
{
    public class ValueParserEventArgs<T> : EventArgs
    {
        public string OriginalContent { get; set; }

        public T ParsedItem { get; set; }
    }

    public delegate void OnValueParsedItem<T>(ValueParserEventArgs<T> e);
}
