using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Neptuo.Templates.Controls
{
    public class GenericControl : BaseContentControl
    {
        public new string TagName { get; set; }

        protected override bool IsSelfClosing
        {
            get { return true; }
        }

        public GenericControl(string tagName)
        {
            TagName = TagName;
        }

        public GenericControl()
        { }

        public override void Render(HtmlTextWriter writer)
        {
            if (!String.IsNullOrEmpty(TagName))
                base.Render(writer);
        }
    }
}
