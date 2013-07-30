﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Neptuo.Templates.Controls
{
    [Html("input", true)]
    [DefaultProperty("Text")]
    public class TextBoxControl : BaseControl
    {
        public string Name
        {
            get { return Attributes["name"]; }
            set { Attributes["name"] = value; }
        }

        public string Text
        {
            get { return Attributes["value"]; }
            set { Attributes["value"] = value; }
        }

        public override void OnInit()
        {
            base.OnInit();

            Attributes["type"] = "text";
        }

        public override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
        }
    }
}
