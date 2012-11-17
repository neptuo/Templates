﻿using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Controls
{
    [DefaultProperty("Text")]
    public class LiteralControl : BaseControl
    {
        public string Text { get; set; }

        protected override void RenderBody(HtmlTextWriter writer)
        {
            writer.Write(Text);
        }
    }
}
