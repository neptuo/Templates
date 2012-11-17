using Neptuo.Web.Framework.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Observers
{
    public interface IObserver
    {
        void OnInit(ObserverEventArgs e);

        void Render(ObserverEventArgs e, HtmlTextWriter writer);
    }

    public class ObserverEventArgs : EventArgs
    {
        public IControl Target { get; set; }
        public bool Cancel { get; set; }

        public ObserverEventArgs(IControl target)
        {
            Target = target;
        }
    }
}
