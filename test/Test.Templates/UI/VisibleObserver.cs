using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Templates.UI
{
    public class VisibleObserver : IControlObserver
    {
        public bool Visible { get; set; }

        public void OnInit(IControlObserverContext context)
        {
            if (!Visible)
                context.Cancel = true;
        }

        public void Render(IControlObserverContext context, IHtmlWriter writer)
        {
            if (!Visible)
                context.Cancel = true;
        }
    }
}
