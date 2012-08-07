using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neptuo.Web.Framework.Annotations;

namespace Neptuo.Web.Framework.Observers
{
    [Observer(Livecycle = ObserverLivecycle.PerAttribute)]
    public class VisibleObserver : IObserver
    {
        public void OnInit(ObserverEventArgs e)
        {
            if (e.AttributeValue.ToString().ToLowerInvariant() == Boolean.FalseString.ToLowerInvariant())
                e.Cancel = true;
        }

        public void Render(ObserverEventArgs e, HtmlTextWriter writer)
        {
            if (e.AttributeValue.ToString().ToLowerInvariant() == Boolean.FalseString.ToLowerInvariant())
                e.Cancel = true;
        }
    }
}
