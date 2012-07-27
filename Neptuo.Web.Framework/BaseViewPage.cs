using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework
{
    public class BaseViewPage : IViewPage
    {
        private ILivecycleObserver livecycleObserver;

        public List<object> Content { get; set; }

        public BaseViewPage(ILivecycleObserver livecycleObserver)
        {
            Content = new List<object>();

            this.livecycleObserver = livecycleObserver;
        }

        public void OnInit()
        {
            foreach (object item in Content)
            {
                IControl control = item as IControl;
                if (control != null)
                    livecycleObserver.Init(control);
            }
        }

        public void Render(HtmlTextWriter writer)
        {
            foreach (object item in Content)
            {
                IControl control = item as IControl;
                if (control != null)
                    livecycleObserver.Render(control, writer);
            }
        }

        public void Dispose()
        {
            foreach (object control in livecycleObserver.GetControls())
                livecycleObserver.Dispose(control);
        }
    }
}
