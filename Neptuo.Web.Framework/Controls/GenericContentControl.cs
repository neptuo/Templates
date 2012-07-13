using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Neptuo.Web.Framework.Controls
{
    public class GenericContentControl : BaseContentControl
    {
        public string TagName { get; set; }

        public GenericContentControl(string tagName)
        {
            TagName = tagName;
        }

        public GenericContentControl()
        { }

        public override void Render(HtmlTextWriter writer)
        {
            if (!String.IsNullOrEmpty(GetTagName()))
                base.Render(writer);
        }

        protected override string GetTagName()
        {
            return TagName;
        }

        protected override bool GetIsSelfClosing()
        {
            return false;
        }
    }
}
