﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Neptuo.Templates;
using Neptuo.Templates.Runtime;

namespace TestConsoleNG.Controls
{
    [Html("input", true)]
    [DefaultProperty("Text")]
    public class TextBoxControl : BaseControl
    {
        public string Name
        {
            get { return HtmlAttributes["name"]; }
            set { HtmlAttributes["name"] = value; }
        }

        public string Text
        {
            get { return HtmlAttributes["value"]; }
            set { HtmlAttributes["value"] = value; }
        }

        public override void OnInit(IComponentManager componentManager)
        {
            base.OnInit(componentManager);

            HtmlAttributes["type"] = "text";
        }

        public override void Render(IHtmlWriter writer)
        {
            base.Render(writer);
        }
    }
}
