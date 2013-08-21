using Neptuo.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestConsoleNG.Controls
{
    [Html("div")]
    public class PanelControl : BaseContentControl
    {
        public string Header { get; set; }

        public PanelControl(IComponentManager componentManager)
            : base(componentManager)
        { }

        protected override void RenderBody(IHtmlWriter writer)
        {
            if (!String.IsNullOrEmpty(Header))
            {
                writer
                    .Tag("div")
                    .Attribute("class", "panel-header")
                    .Content(Header)
                    .CloseTag();
            }

            base.RenderBody(writer);
        }
    }
}
