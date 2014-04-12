using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates
{
    partial class ComponentManager
    {
        internal abstract class ComponentEntryBase
        {
            public virtual object Control { get; set; }
            public virtual Delegate PropertyBinder { get; set; }

            public List<ObserverInfo> Observers { get; private set; }
            public List<OnInitComplete> InitComplete { get; private set; }

            public bool ArePropertiesBound { get; set; }
            public bool IsInited { get; set; }
            public bool IsDisposed { get; set; }

            public ComponentEntryBase()
            {
                Observers = new List<ObserverInfo>();
                InitComplete = new List<OnInitComplete>();
            }

            public abstract void BindProperties();
        }

        internal class ComponentEntry<T> : ComponentEntryBase
        {
            private T control;
            private Action<T> propertyBinder;

            public override object Control
            {
                get { return control; }
                set { control = (T)value; }
            }

            public override Delegate PropertyBinder
            {
                get { return propertyBinder; }
                set { propertyBinder = (Action<T>)value; }
            }

            public ComponentEntry()
            {

            }

            public override void BindProperties()
            {
                if (propertyBinder != null)
                {
                    propertyBinder(control);
                    ArePropertiesBound = true;
                }
            }
        }
    }
}
