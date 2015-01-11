using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Neptuo.Templates;
using Neptuo.Templates.Runtime;

namespace Test.Templates.Controls
{
    [DefaultProperty("Text")]
    public class LiteralControl : BaseControl
    {
        public string Text { get; set; }

        protected override void RenderBody(IHtmlWriter writer)
        {
            writer.Content(Text);
        }
    }
}
