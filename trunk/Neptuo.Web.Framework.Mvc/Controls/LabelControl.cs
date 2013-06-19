using Neptuo.Web.Framework.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Neptuo.Web.Framework.Mvc.Controls
{
    public class LabelControl : IControl
    {
        public string Property { get; set; }

        public void OnInit()
        { }

        public void Render(HtmlTextWriter writer)
        {
            HtmlHelper html = new HtmlHelper(View.ViewContext, View.ViewDataContainer);
            writer.Write(html.Label(Property));
        }
    }
}
