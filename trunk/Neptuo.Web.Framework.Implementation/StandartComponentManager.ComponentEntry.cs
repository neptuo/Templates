using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework
{
    partial class StandartComponentManager
    {
        internal class ComponentEntry
        {
            public object Parent { get; set; }
            public object Control { get; set; }
            public Action PropertyBinder { get; set; }

            public Dictionary<string, List<object>> Properties { get; set; }
            public List<ObserverInfo> Observers { get; set; }

            public bool ArePropertiesBound { get; set; }
            public bool IsInited { get; set; }
            public bool IsDisposed { get; set; }

            public ComponentEntry()
            {
                Properties = new Dictionary<string, List<object>>();
                Observers = new List<ObserverInfo>();
            }
        }
    }
}
