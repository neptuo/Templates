using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Test.Templates.UI.Data;

namespace Test.Templates.UI
{
    public class DataContextObserver : IControlObserver
    {
        private DataStorage dataStorage;
        
        public object DataContext { get; set; }

        public DataContextObserver(DataStorage dataStorage)
        {
            this.dataStorage = dataStorage;
        }

        public void OnInit(IControlObserverContext context)
        {
            context.ComponentManager.AttachInitComplete(context.Target, OnInitComplete);
            dataStorage.Push(DataContext);
        }

        protected void OnInitComplete(IControl target)
        {
            dataStorage.Pop();
        }

        public void Render(IControlObserverContext context, IHtmlWriter writer)
        { }
    }
}
