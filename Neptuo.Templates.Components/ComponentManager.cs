using Neptuo.Templates.Controls;
using Neptuo.Templates.Observers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates
{
    public partial class ComponentManager : IComponentManager
    {
        private Dictionary<object, BaseComponentEntry> entries = new Dictionary<object, BaseComponentEntry>();

        public ComponentManager()
        {

        }

        public virtual void AddComponent<T>(T component, Action<T> propertyBinder)
        {
            BaseComponentEntry entry = new ComponentEntry<T>
            {
                Control = component,
                ArePropertiesBound = propertyBinder == null,
                PropertyBinder = propertyBinder
            };
            entries.Add(component, entry);
        }

        public virtual void AttachObserver<T>(IControl control, T observer, Action<T> propertyBinder)
            where T : IObserver
        {
            //throw new LivecycleException("Control is not registered, unnable to register observer");
            if (!entries.ContainsKey(control))
                return;

            entries[control].Observers.Add(new ObserverInfo<T>(observer, propertyBinder));
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
            BaseComponentEntry entry = entries[control];
            if (entry.IsInited)
                return;

            //throw new LivecycleException("Control is already disposed!");
            if (entry.IsDisposed)
                return;

            BeforeInitComponent(entry.Control);
            
            IControl target = entry.Control as IControl;
            if (target != null)
                BeforeInitControl(target);

            if (!entry.ArePropertiesBound)
                entry.BindProperties();

            entry.IsInited = true;

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
                        info.BindProperties();
                        info.ArePropertiesBound = true;
                    }
                    info.Observer.OnInit(args);

                    if (args.Cancel)
                        canInit = false;
                }
            }

            if (canInit)
                target.OnInit();

            if (entry.InitComplete.Count > 0)
            {
                ControlInitCompleteEventArgs args = new ControlInitCompleteEventArgs(target);
                foreach (OnInitComplete handler in entry.InitComplete)
                    handler(args);
            }

            AfterInitControl(target);
        }

        protected virtual void BeforeInitComponent(object component)
        { }

        protected virtual void BeforeInitControl(IControl control)
        { }

        protected virtual void AfterInitControl(IControl control)
        { }

        public void Render(object control, IHtmlWriter writer)
        {
            if(control == null)
                return;

            if (control.GetType().FullName == typeof(String).FullName)
            {
                BeforeRenderComponent(control, writer);
                writer.Content(control);
                return;
            }

            //throw new LivecycleException("Not registered control!");
            if (!entries.ContainsKey(control))
                return;

            BaseComponentEntry entry = entries[control];

            if (!entry.IsInited)
                Init(control);

            //throw new LivecycleException("Control is already disposed!");
            if (entry.IsDisposed)
                return;

            BeforeRenderComponent(entry.Control, writer);

            IControl target = entry.Control as IControl;
            if (target == null)
                return;

            BeforeRenderControl(target, writer);

            bool canRender = true;
            if (entry.Observers.Count > 0)
            {
                ObserverEventArgs args = new ObserverEventArgs(target);
                foreach (ObserverInfo info in entry.Observers)
                {
                    if (!info.ArePropertiesBound)
                    {
                        info.BindProperties();
                        info.ArePropertiesBound = true;
                    }
                    info.Observer.Render(args, writer);

                    if (args.Cancel)
                        canRender = false;
                }
            }

            if (canRender)
                DoRenderControl(target, writer);

            AfterRenderControl(target, writer);
        }

        protected virtual void BeforeRenderComponent(object component, IHtmlWriter writer)
        { }

        protected virtual void BeforeRenderControl(IControl control, IHtmlWriter writer)
        { }

        protected virtual void DoRenderControl(IControl control, IHtmlWriter writer)
        {
            control.Render(writer);
        }

        protected virtual void AfterRenderControl(IControl control, IHtmlWriter writer)
        { }

        public void Dispose(object component)
        {
            //throw new LivecycleException("Not registered control!");
            if (!entries.ContainsKey(component))
                return;

            BaseComponentEntry entry = entries[component];

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

        protected virtual void BeforeDisposeComponent(object component)
        { }

        protected virtual void BeforeDisposeControl(IControl control)
        { }
    }
}
