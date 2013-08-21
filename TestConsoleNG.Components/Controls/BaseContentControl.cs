using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Collections;
using Neptuo.Templates.Controls;
using Neptuo.Templates;

namespace TestConsoleNG.Controls
{
    [DefaultProperty("Content")]
    public abstract class BaseContentControl : BaseControl, IContentControl
    {
        protected override bool IsSelfClosing
        {
            get
            {
                if (Content != null && Content.Count != 0)
                    return false;

                return base.IsSelfClosing;
            }
        }

        public ICollection<object> Content { get; set; }

        public BaseContentControl(IComponentManager componentManager)
            : base(componentManager)
        { }

        public override void OnInit()
        {
            base.OnInit();

            if (Content != null)
            {
                foreach (object item in Content)
                    ComponentManager.Init(item);
            }
        }

        protected override void RenderBody(IHtmlWriter writer)
        {
            if (Content != null)
            {
                foreach (object item in Content)
                    ComponentManager.Render(item, writer);
            }
        }
    }
}
