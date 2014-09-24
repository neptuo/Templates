﻿using Neptuo.Templates;
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
        public ITemplate Template { get; set; }

        public PanelControl(IComponentManager componentManager)
            : base(componentManager)
        { }

        public override void OnInit()
        {
            base.OnInit();

            if (Template != null)
            {
                Init(Template);

                for (int i = 0; i < 5; i++)
                {
                    ITemplateContent templateContent = Template.CreateInstance();
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