using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Parser.ExtensionContent
{
    public class ExtensionEventArgs : EventArgs
    {
        public int StartPosition { get; set; }

        public int EndPosition { get; set; }

        public string OriginalContent { get; set; }

        public Extension Extension { get; set; }
    }
}
