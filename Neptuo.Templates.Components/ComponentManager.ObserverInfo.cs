using Neptuo.Templates.Observers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates
{
    partial class ComponentManager
    {
        internal class ObserverInfo
        {
            public IObserver Observer { get; set; }
            public Action PropertyBinder { get; set; }

            public bool ArePropertiesBound { get; set; }

            public ObserverInfo(IObserver observer, Action propertyBinder)
            {
                Observer = observer;
                PropertyBinder = propertyBinder;
            }
        }
    }
}
