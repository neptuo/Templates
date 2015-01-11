using Neptuo.Templates;
using Neptuo.Templates.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.Templates.Controls
{
    [Html("div")]
    public class PanelControl : BaseContentControl
    {
        public string Header { get; set; }
        public ITemplate Template { get; set; }

        public override void OnInit(IComponentManager componentManager)
        {
            base.OnInit(componentManager);

            if (Template != null)
            {
                Init(Template);

                for (int i = 0; i < 5; i++)
                {
                    ITemplateContent templateContent = Template.CreateInstance(componentManager);
                    ComponentManager.Init(templateContent);
                    Content.Add(templateContent);
                }
            }
        }

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
