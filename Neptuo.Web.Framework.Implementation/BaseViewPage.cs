using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework
{
    public class BaseViewPage : IViewPage
    {
        private IComponentManager componentManager;

        public ICollection<object> Content { get; set; }

        public BaseViewPage(IComponentManager componentManager)
        {
            this.componentManager = componentManager;
        }

        public void OnInit()
        {
            foreach (object item in Content)
            {
                IControl control = item as IControl;
                if (control != null)
                    componentManager.Init(control);
            }
        }

        public void Render(HtmlTextWriter writer)
        {
            foreach (object item in Content)
            {
                IControl control = item as IControl;
                if (control != null)
                    componentManager.Render(control, writer);
            }
        }

        public void Dispose()
        {
            foreach (object control in componentManager.GetComponents())
                componentManager.Dispose(control);
        }
    }
}
