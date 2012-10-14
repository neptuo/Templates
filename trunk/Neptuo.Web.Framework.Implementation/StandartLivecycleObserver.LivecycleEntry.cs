using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework
{
    partial class StandartLivecycleObserver
    {
        internal class LivecycleEntry
        {
            public object Parent { get; set; }
            public object Control { get; set; }
            public Action PropertyBinder { get; set; }

            public List<ObserverInfo> Observers { get; set; }

            public bool ArePropertiesBound { get; set; }
            public bool IsInited { get; set; }
            public bool IsDisposed { get; set; }

            public LivecycleEntry()
            {
                Observers = new List<ObserverInfo>();
            }
        }
    }
}
