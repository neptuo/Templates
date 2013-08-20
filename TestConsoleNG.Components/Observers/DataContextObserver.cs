using Neptuo.Templates;
using Neptuo.Templates.Observers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using TestConsoleNG.Data;

namespace TestConsoleNG.Observers
{
    [Observer(ObserverLivecycle.PerPage)]
    public class DataContextObserver : IObserver
    {
        private IComponentManager componentManager;
        private DataStorage dataStorage;
        
        public object DataContext { get; set; }

        public DataContextObserver(IComponentManager componentManager, DataStorage dataStorage)
        {
            this.componentManager = componentManager;
            this.dataStorage = dataStorage;
        }

        public void OnInit(ObserverEventArgs e)
        {
            componentManager.AttachInitComplete(e.Target, OnInitComplete);
            dataStorage.Push(DataContext);
        }

        protected void OnInitComplete(ControlInitCompleteEventArgs e)
        {
            dataStorage.Pop();
        }

        public void Render(ObserverEventArgs e, HtmlTextWriter writer)
        { }
    }
}
