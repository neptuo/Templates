using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Parser
{
    public class ContentParserEventArgs<T> : EventArgs
    {
        public int StartPosition { get; set; }

        public int EndPosition { get; set; }

        public string OriginalContent { get; set; }

        public T ParsedItem { get; set; }
    }

    public class ContentParserDoneEventArgs : EventArgs
    {
        public string OriginalContent { get; set; }
    }
}
