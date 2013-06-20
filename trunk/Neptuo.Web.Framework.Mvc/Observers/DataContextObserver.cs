using Neptuo.Web.Framework.Mvc.Data;
using Neptuo.Web.Framework.Observers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Mvc.Observers
{
    [Observer(ObserverLivecycle.PerPage)]
    public class DataContextObserver : IObserver
    {
        private IComponentManager componentManager;
        private DataStorage storage;

        public object DataContext { get; set; }

        public DataContextObserver(IComponentManager componentManager, DataStorage storage)
        {
            this.componentManager = componentManager;
            this.storage = storage;
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
