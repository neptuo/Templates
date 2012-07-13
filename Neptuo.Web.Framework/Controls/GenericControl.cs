using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Neptuo.Web.Framework.Controls
{
    public class GenericControl : BaseContentControl
    {
        public string TagName { get; set; }

        public GenericControl(string tagName)
        {
            TagName = TagName;
        }

        public GenericControl()
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
            return true;
        }
    }
}
