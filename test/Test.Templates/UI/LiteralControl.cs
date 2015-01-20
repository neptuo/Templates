using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Test.Templates.UI
{
    [DefaultProperty("Text")]
    public class LiteralControl : IControl
    {
        public string Text { get; set; }

        public void OnInit(IComponentManager componentManager)
        { }

        public void Render(IHtmlWriter writer)
        {
            writer.Content(Text);
        }
    }
}
