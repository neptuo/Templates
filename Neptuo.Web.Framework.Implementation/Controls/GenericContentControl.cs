using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Neptuo.Web.Framework.Controls
{
    public class GenericContentControl : BaseContentControl
    {
        public new string TagName
        {
            get { return base.TagName; }
            set { base.TagName = value; }
        }

        public GenericContentControl(string tagName)
        {
            TagName = tagName;
        }

        public GenericContentControl()
        { 
            IsSelfClosing = false;
        }

        public override void Render(HtmlTextWriter writer)
        {
            if (!String.IsNullOrEmpty(TagName))
                base.Render(writer);
        }
    }
}
