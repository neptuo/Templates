using Neptuo.Web.Framework.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Neptuo.Web.Framework.Mvc.Controls
{
    public class LabeledEditor : IControl
    {
        private LabelControl label;
        private EditorControl editor;

        public string Property { get; set; }

        public void OnInit()
        {
            label = new LabelControl { Property = Property };
            label.OnInit();
            editor = new EditorControl { Property = Property };
            editor.OnInit();
        }

        public void Render(HtmlTextWriter writer)
        {
            writer.WriteFullBeginTag("div");
            label.Render(writer);
            editor.Render(writer);
            writer.WriteEndTag("div");
        }
    }
}
