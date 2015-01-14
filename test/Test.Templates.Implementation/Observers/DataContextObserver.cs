using Neptuo.Templates;
using Neptuo.Templates.Observers;
using Neptuo.Templates.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Test.Templates.Data;

namespace Test.Templates.Observers
{
    public class DataContextObserver : IObserver
    {
        private DataStorage dataStorage;
        
        public object DataContext { get; set; }

        public DataContextObserver(DataStorage dataStorage)
        {
            this.dataStorage = dataStorage;
        }

        public void OnInit(ObserverEventArgs e)
        {
            e.ComponentManager.AttachInitComplete(e.Target, OnInitComplete);
            dataStorage.Push(DataContext);
        }

        protected void OnInitComplete(ControlInitCompleteEventArgs e)
        {
            dataStorage.Pop();
        }

        public void Render(ObserverEventArgs e, IHtmlWriter writer)
        { }
    }
}
