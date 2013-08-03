using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Controls
{
    [DefaultProperty("Text")]
    public class LiteralControl : BaseControl
    {
        public string Text { get; set; }

        public LiteralControl(IComponentManager componentManager)
            : base(componentManager)
        { }

        protected override void RenderBody(HtmlTextWriter writer)
        {
            writer.Write(Text);
        }
    }
}
