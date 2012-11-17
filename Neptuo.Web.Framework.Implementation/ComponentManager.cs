using Neptuo.Web.Framework.Controls;
using Neptuo.Web.Framework.Observers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework
{
    public partial class ComponentManager : IComponentManager
    {
        private Dictionary<object, ComponentEntry> entries = new Dictionary<object, ComponentEntry>();

        public void AddComponent(object component, Action propertyBinder)
        {
            ComponentEntry entry = new ComponentEntry
            {
                Control = component,
                ArePropertiesBound = propertyBinder == null,
                PropertyBinder = propertyBinder
            };
            entries.Add(component, entry);
        }

        public void AttachObserver(IControl control, IObserver observer, Action propertyBinder)
        {
            //throw new LivecycleException("Control is not registered, unnable to register observer");
            if (!entries.ContainsKey(control))
                return;

            entries[control].Observers.Add(new ObserverInfo(observer, propertyBinder));
        }

        public void AttachInitComplete(IControl control, OnInitComplete handler)
        {
            if (entries.ContainsKey(control))
                entries[control].InitComplete.Add(handler);
        }

        public void Init(object control)
        {
            //throw new LivecycleException("Not registered control!");
            if (control == null || !entries.ContainsKey(control))
                return;

            //throw new LivecycleException("Control is already inited!");
            ComponentEntry entry = entries[control];
            if (entry.IsInited)
                return;

            //throw new LivecycleException("Control is already disposed!");
            if (entry.IsDisposed)
                return;

            if (!entry.ArePropertiesBound)
            {
                entry.PropertyBinder();
                entry.ArePropertiesBound = true;
            }

            entry.IsInited = true;

            IControl target = entry.Control as IControl;
            if (target == null)
                return;

            bool canInit = true;
            if (entry.Observers.Count > 0)
            {
                ObserverEventArgs args = new ObserverEventArgs(target);
                foreach (ObserverInfo info in entry.Observers)
                {
                    if (!info.ArePropertiesBound)
                    {
                        info.PropertyBinder();
                        info.ArePropertiesBound = true;
                    }
                    info.Observer.OnInit(args);

                    if (args.Cancel)
                        canInit = false;
                }
            }

            if (canInit)
                target.OnInit();

            if (entry.InitComplete.Any())
            {
                ControlInitCompleteEventArgs args = new ControlInitCompleteEventArgs(target);
                foreach (OnInitComplete handler in entry.InitComplete)
                    handler(args);
            }
        }

        public void Render(object control, HtmlTextWriter writer)
        {
            if(control == null)
                return;

            if (control.GetType() == typeof(String))
                writer.Write(control);

            //throw new LivecycleException("Not registered control!");
            if (!entries.ContainsKey(control))
                return;

            ComponentEntry entry = entries[control];

            if (!entry.IsInited)
                Init(control);

            //throw new LivecycleException("Control is already disposed!");
            if (entry.IsDisposed)
                return;

            IControl target = entry.Control as IControl;
            if (target == null)
                return;

            bool canRender = true;
            if (entry.Observers.Count > 0)
            {
                ObserverEventArgs args = new ObserverEventArgs(target);
                foreach (ObserverInfo info in entry.Observers)
                {
                    if (!info.ArePropertiesBound)
                    {
                        info.PropertyBinder();
                        info.ArePropertiesBound = true;
                    }
                    info.Observer.Render(args, writer);

                    if (args.Cancel)
                        canRender = false;
                }
            }

            if (canRender)
                target.Render(writer);
        }

        public void Dispose(object component)
        {
            //throw new LivecycleException("Not registered control!");
            if (!entries.ContainsKey(component))
                return;

            ComponentEntry entry = entries[component];

            if (!entry.IsInited)
                Init(component);

            //throw new LivecycleException("Control is already disposed!");
            if (entry.IsDisposed)
                return;

            IDisposable target = entry.Control as IDisposable;
            if (target != null)
            {
                target.Dispose();
                entry.IsDisposed = true;
            }
        }
    }
}
