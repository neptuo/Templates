using Neptuo.Web.Framework.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Neptuo.Web.Framework.Mvc.Controls
{
    public class EditorControl : IControl
    {
        private HtmlHelper htmlHelper;

        public string Property { get; set; }

        public EditorControl(HtmlHelper htmlHelper)
        {
            this.htmlHelper = htmlHelper;
        }

        public void OnInit()
        { }

        public void Render(HtmlTextWriter writer)
        {
            writer.Write(htmlHelper.Editor(Property));
        }
    }
}
