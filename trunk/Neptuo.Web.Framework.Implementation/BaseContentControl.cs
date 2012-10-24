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
        [Dependency]
        public IComponentManager ComponentManager { get; set; }

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
                    ComponentManager.Init(control);
            }
        }

        protected override void RenderBody(HtmlTextWriter writer)
        {
            foreach (object item in Content)
            {
                IControl control = item as IControl;
                if (control != null)
                    ComponentManager.Render(control, writer);
            }
        }

        protected override bool GetIsSelfClosing()
        {
            if (Content != null & Content.Count != 0)
                return false;

            return base.GetIsSelfClosing();
        }
    }
}
