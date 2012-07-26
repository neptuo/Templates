using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Neptuo.Web.Framework
{
    public class HtmlTextWriter : System.Web.UI.HtmlTextWriter
    {
        public new TextWriter InnerWriter { get; protected set; }

        public HtmlTextWriter(TextWriter writer)
            : base(writer)
        {
            
        }

        public override Encoding Encoding
        {
            get { return InnerWriter.Encoding; }
        }
    }
}
