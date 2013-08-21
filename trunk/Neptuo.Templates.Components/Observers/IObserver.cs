using Neptuo.Templates.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Observers
{
    public interface IObserver
    {
        void OnInit(ObserverEventArgs e);

        void Render(ObserverEventArgs e, IHtmlWriter writer);
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
