using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Observers
{
    [Observer(ObserverLivecycle.PerPage)]
    public class DataContextObserver : IObserver
    {
        private Stack<object> storage = new Stack<object>();

        public IComponentManager ComponentManager { get; set; }

        public object DataContext { get; set; }

        public void OnInit(ObserverEventArgs e)
        {
            ComponentManager.AttachInitComplete(e.Target, OnInitComplete);
            storage.Push(DataContext);
            Models.CurrentModel.Push(DataContext);//TODO: Delete this line.
        }

        protected void OnInitComplete(ControlInitCompleteEventArgs e)
        {
            storage.Pop();
            Models.CurrentModel.Pop();//TODO: Delete this line.
        }

        public void Render(ObserverEventArgs e, HtmlTextWriter writer)
        {
            
        }
    }
}
