using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Neptuo.Web.Framework.Controls
{
    public class GenericContentControl : BaseContentControl
    {
        public new string TagName { get; set; }

        protected override bool IsSelfClosing
        {
            get { return false; }
        }

        public GenericContentControl(string tagName)
        {
            TagName = tagName;
        }

        public GenericContentControl()
        { }

        public override void Render(HtmlTextWriter writer)
        {
            if (!String.IsNullOrEmpty(TagName))
                base.Render(writer);
        }
    }
}
