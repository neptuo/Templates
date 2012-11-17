using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Observers
{
    [Observer(ObserverLivecycle.PerControl)]
    public class VisibleObserver : IObserver
    {
        public bool Visible { get; set; }

        public void OnInit(ObserverEventArgs e)
        {
            if (!Visible)
                e.Cancel = true;
        }

        public void Render(ObserverEventArgs e, HtmlTextWriter writer)
        {
            if (!Visible)
                e.Cancel = true;
        }
    }
}
