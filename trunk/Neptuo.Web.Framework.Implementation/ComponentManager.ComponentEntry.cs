using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework
{
    partial class ComponentManager
    {
        internal class ComponentEntry
        {
            public object Control { get; set; }
            public Action PropertyBinder { get; set; }

            public List<ObserverInfo> Observers { get; private set; }
            public List<OnInitComplete> InitComplete { get; private set; }

            public bool ArePropertiesBound { get; set; }
            public bool IsInited { get; set; }
            public bool IsDisposed { get; set; }

            public ComponentEntry()
            {
                Observers = new List<ObserverInfo>();
                InitComplete = new List<OnInitComplete>();
            }
        }
    }
}
