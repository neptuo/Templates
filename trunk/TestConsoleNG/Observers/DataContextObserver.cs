using Neptuo.Templates;
using Neptuo.Templates.Observers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestConsoleNG.Observers
{
    [Observer(ObserverLivecycle.PerPage)]
    public class DataContextObserver : IObserver
    {
        private IComponentManager componentManager;

        private Stack<object> storage = new Stack<object>();

        public object DataContext { get; set; }

        public DataContextObserver(IComponentManager componentManager)
        {
            this.componentManager = componentManager;
        }

        public void OnInit(ObserverEventArgs e)
        {
            componentManager.AttachInitComplete(e.Target, OnInitComplete);
            storage.Push(DataContext);
        }

        protected void OnInitComplete(ControlInitCompleteEventArgs e)
        {
            storage.Pop();
        }

        public void Render(ObserverEventArgs e, HtmlTextWriter writer)
        { }
    }
}
