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

        protected override bool IsSelfClosing
        {
            get
            {
                if (Content != null & Content.Count != 0)
                    return false;

                return base.IsSelfClosing;
            }
        }

        public ICollection<object> Content { get; set; }

        public override void OnInit()
        {
            base.OnInit();

            foreach (object item in Content)
                ComponentManager.Init(item);
        }

        protected override void RenderBody(HtmlTextWriter writer)
        {
            foreach (object item in Content)
                ComponentManager.Render(item, writer);
        }
    }
}
