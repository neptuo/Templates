using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Neptuo.Templates;

namespace TestConsoleNG.Controls
{
    public class GenericContentControl : BaseContentControl
    {
        public new string TagName
        {
            get { return base.TagName; }
            set { base.TagName = value; }
        }

        public GenericContentControl(string tagName, IComponentManager componentManager)
            : this(componentManager)
        {
            TagName = tagName;
        }

        public GenericContentControl(IComponentManager componentManager)
            : base(componentManager)
        { 
            //IsSelfClosing = false;
        }

        public override void Render(IHtmlWriter writer)
        {
            if (!String.IsNullOrEmpty(TagName))
                base.Render(writer);
        }
    }
}
