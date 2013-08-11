﻿using Neptuo.Templates;
using Neptuo.Templates.Observers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleNG.Observers
{
    public class VisibleObserver : IObserver
    {
        public bool Visible { get; set; }

        public void OnInit(ObserverEventArgs e)
        {
            if (!Visible)
                e.Cancel = true;
        }

        public void Render(ObserverEventArgs e, HtmlTextWriter writer)
        {
            if (!Visible)
                e.Cancel = true;
        }
    }
}