using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Neptuo.Web.Framework.Annotations;
using System.IO;

namespace Neptuo.Web.Framework
{
    [DefaultProperty("Content")]
    public abstract class BaseContentControl : BaseControl, IContentControl
    {
        public List<object> Content { get; set; }

        public BaseContentControl()
        {
            Content = new List<object>();
        }

        public override void OnInit()
        {
            base.OnInit();

            foreach (object item in Content)
            {
                IControl control = item as IControl;
                if (control != null)
                    control.OnInit();
            }
        }

        protected override void RenderBody(HtmlTextWriter writer)
        {
            foreach (object item in Content)
            {
                IControl control = item as IControl;
                if (control != null)
                    control.Render(writer);
            }
        }
    }
}
