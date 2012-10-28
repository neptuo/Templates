using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework
{
    public partial class StandartComponentManager : IComponentManager
    {
        private Dictionary<object, ComponentEntry> entries = new Dictionary<object, ComponentEntry>();
        //private Dictionary<object, Dictionary<string, List<object>>> children = new Dictionary<object, Dictionary<string, List<object>>>();
        private ComponentEntry rootComponent;

        public StandartComponentManager()
        {
            ComponentEntry entry = new ComponentEntry
            {
                Control = rootComponent,
                Parent = null,
                ArePropertiesBound = true,
                PropertyBinder = null
            };
            //entries.Add(rootComponent, entry);
            //RegisterProperty(rootComponent, "Root");
        }

        public IEnumerable<object> GetComponents()
        {
            return entries.Keys;
        }

        public ICollection<object> GetComponents(object owner, string propertyName)
        {
            if (!entries.ContainsKey(owner))
                throw new LivecycleException("Owner is not registered!");

            if (!entries[owner].Properties.ContainsKey(propertyName))
                RegisterProperty(owner, propertyName);

            return entries[owner].Properties[propertyName];
        }

        public void RegisterProperty(object owner, string propertyName)
        {
            if (!entries.ContainsKey(owner))
                throw new LivecycleException("Parent is not registered!");

            if (!entries[owner].Properties.ContainsKey(propertyName))
                entries[owner].Properties.Add(propertyName, new List<object>());
        }

        public void SetRootComponent(object component, Action propertyBinder)
        {
            if (rootComponent != null)
                entries.Remove(rootComponent.Control);

            rootComponent = new ComponentEntry
            {
                Control = component,
                PropertyBinder = propertyBinder,
                ArePropertiesBound = propertyBinder == null
            };
            entries.Add(component, rootComponent);
        }

        public void AddComponent(object parent, string propertyName, object component, Action propertyBinder)
        {
            if (parent == null)
                parent = rootComponent.Control;

            if (!entries.ContainsKey(parent))
                throw new LivecycleException("Parent is not registered!");

            if(!entries[parent].Properties.ContainsKey(propertyName))
                RegisterProperty(parent, propertyName);

            //if (!(control is IControl) && !(control is IViewPage))
            //    return;

            ComponentEntry entry = new ComponentEntry
            {
                Control = component,
                Parent = parent,
                ArePropertiesBound = propertyBinder == null,
                PropertyBinder = propertyBinder
            };
            entries.Add(component, entry);
            entries[parent].Properties[propertyName].Add(component);
        }

        public bool RemoveComponent(object parent, string propertyName, object component)
        {
            if (parent == null)
                parent = rootComponent;

            if (!entries.ContainsKey(parent))
                throw new LivecycleException("Parent is not registered!");

            if (entries[parent].Properties.ContainsKey(propertyName))
            {
                entries.Remove(component);
                return entries[parent].Properties[propertyName].Remove(component);
            }
            return false;
        }

        public void AttachObserver(IControl control, IObserver observer, Action propertyBinder)
        {
            if(!entries.ContainsKey(control))
                throw new LivecycleException("Control is not registered, unnable to register observer");

            entries[control].Observers.Add(new ObserverInfo(observer, propertyBinder));
        }

        public void Init(object control)
        {
            if (!entries.ContainsKey(control))
                throw new LivecycleException("Not registered control!");

            ComponentEntry entry = entries[control];
            if (entry.IsInited)
                throw new LivecycleException("Control is already inited!");

            if (entry.IsDisposed)
                throw new LivecycleException("Control is already disposed!");

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
        }

        public void Render(object control, HtmlTextWriter writer)
        {
            if (!entries.ContainsKey(control))
                throw new LivecycleException("Not registered control!");

            ComponentEntry entry = entries[control];

            if (!entry.IsInited)
                Init(control);

            if (entry.IsDisposed)
                throw new LivecycleException("Control is already disposed!");

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
            if (!entries.ContainsKey(component))
                throw new LivecycleException("Not registered control!");

            ComponentEntry entry = entries[component];

            if (!entry.IsInited)
                Init(component);

            if (entry.IsDisposed)
                throw new LivecycleException("Control is already disposed!");

            IDisposable target = entry.Control as IDisposable;
            if (target != null)
            {
                target.Dispose();
                entry.IsDisposed = true;
            }
        }
    }
}
