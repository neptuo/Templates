using Neptuo.Templates.Observers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates
{
    partial class ComponentManager
    {
        internal abstract class ObserverInfo
        {
            public virtual IObserver Observer { get; set; }
            public virtual Delegate PropertyBinder { get; set; }

            public bool ArePropertiesBound { get; set; }

            public ObserverInfo(IObserver observer, Delegate propertyBinder)
            {
                Observer = observer;
                PropertyBinder = propertyBinder;
            }

            public abstract void BindProperties();
        }

        internal class ObserverInfo<T> : ObserverInfo
            where T : IObserver
        {
            private T observer;
            private Action<T> propertyBinder;

            public override IObserver Observer
            {
                get { return observer; }
                set { observer = (T)value; }
            }

            public override Delegate PropertyBinder
            {
                get { return propertyBinder; }
                set { propertyBinder = (Action<T>)value; }
            }

            public ObserverInfo(T observer, Action<T> propertyBinder)
                : base(observer, propertyBinder)
            { }

            public override void BindProperties()
            {
                if (propertyBinder != null)
                {
                    propertyBinder(observer);
                    ArePropertiesBound = true;
                }
            }
        }
    }
}
