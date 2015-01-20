using Neptuo.Templates;
using Neptuo.Templates.Observers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Templates.Observers
{
    public class VisibleObserver : IControlObserver
    {
        public bool Visible { get; set; }

        public void OnInit(DefaultControlObserverContext e)
        {
            if (!Visible)
                e.Cancel = true;
        }

        public void Render(DefaultControlObserverContext e, IHtmlWriter writer)
        {
            if (!Visible)
                e.Cancel = true;
        }
    }
}
