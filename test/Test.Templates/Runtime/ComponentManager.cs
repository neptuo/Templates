﻿using Neptuo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDisposable = Neptuo.IDisposable;

namespace Test.Templates.Runtime
{
    /// <summary>
    /// Standart implementation of <see cref="IComponentManager"/>.
    /// </summary>
    public partial class ComponentManager : IComponentManager
    {
        /// <summary>
        /// List of register objects.
        /// Key is target object, value is registration structure, <see cref="ComponentEntryBase"/>.
        /// </summary>
        private Dictionary<object, ComponentEntryBase> entries = new Dictionary<object, ComponentEntryBase>();

        public virtual void AddComponent<T>(T component, Action<T> propertyBinder)
        {
            Ensure.NotNull(component, "component");

            ComponentEntryBase entry = new ComponentEntry<T>
            {
                Control = component,
                ArePropertiesBound = propertyBinder == null,
                PropertyBinder = propertyBinder
            };
            entries.Add(component, entry);
        }

        public virtual void AttachObserver<T>(IControl control, T observer, Action<T> propertyBinder)
            where T : IControlObserver
        {
            Ensure.NotNull(control, "control");
            Ensure.NotNull(observer, "observer");

            if (!entries.ContainsKey(control))
                return;

            entries[control].Observers.Add(new ObserverInfo<T>(observer, propertyBinder));
        }

        public void AttachInitComplete(IControl control, Action<IControl> handler)
        {
            Ensure.NotNull(control, "control");
            Ensure.NotNull(handler, "handler");

            if (entries.ContainsKey(control))
                entries[control].InitComplete.Add(handler);
        }

        public void Init(object control)
        {
            // control is null, continue on processing view.
            if (control == null)
                return;

            if (!entries.ContainsKey(control))
            {
                // Try init on not registered control
                BeforeInitComponent(control);

                IControl targetControl = control as IControl;
                if (targetControl != null)
                {
                    BeforeInitControl(targetControl);
                    targetControl.OnInit(this);
                }
                return;
            }

            // if control is already inited, continue on processing view.
            ComponentEntryBase entry = entries[control];
            if (entry.IsInited)
                return;

            // if control is already disposed, continue on processing view.
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

            if (ExecuteObservers(entry))
                target.OnInit(this);

            if (entry.InitComplete.Count > 0)
            {
                foreach (Action<IControl> handler in entry.InitComplete)
                    handler(target);
            }

            AfterInitControl(target);
        }

        /// <summary>
        /// Executes all registered observer on <paramref name="entry"/>.
        /// </summary>
        /// <param name="entry">Registration entry.</param>
        /// <returns>Whether execution on <paramref name="entry"/> should be canceled.</returns>
        private bool ExecuteObservers(ComponentEntryBase entry)
        {
            bool canInit = true;
            if (entry.Observers.Count > 0)
            {
                DefaultControlObserverContext args = new DefaultControlObserverContext(entry.Control as IControl, this);
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

            return canInit;
        }

        /// <summary>
        /// Called before initing any component.
        /// </summary>
        /// <param name="component">Target component.</param>
        protected virtual void BeforeInitComponent(object component)
        { }

        /// <summary>
        /// Called before initing control (<see cref="IControl"/>).
        /// </summary>
        /// <param name="control">Target control.</param>
        protected virtual void BeforeInitControl(IControl control)
        { }

        /// <summary>
        /// Called after (whole) init phase was completed on <paramref name="control"/>.
        /// </summary>
        /// <param name="control">Target control.</param>
        protected virtual void AfterInitControl(IControl control)
        { }

        public void Render(object control, IHtmlWriter writer)
        {
            if(control == null)
                return;

            // if control is string, simply write to writer
            if (control.GetType().FullName == typeof(String).FullName)
            {
                BeforeRenderComponent(control, writer);
                writer.Content(control);
                return;
            }

            if (!entries.ContainsKey(control))
            {
                // Try render not registered control.
                IControl targetControl = control as IControl;
                if (targetControl != null)
                {
                    BeforeRenderControl(targetControl, writer);
                    DoRenderControl(targetControl, writer);
                    AfterRenderControl(targetControl, writer);
                }
                return;
            }

            ComponentEntryBase entry = entries[control];

            // if not inited, init it.
            if (!entry.IsInited)
                Init(control);

            // if dispose, continue processing view.
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
                DefaultControlObserverContext args = new DefaultControlObserverContext(target, this);
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

        /// <summary>
        /// Called before rendering any component.
        /// </summary>
        /// <param name="component">Target component.</param>
        /// <param name="writer">Output writer.</param>
        protected virtual void BeforeRenderComponent(object component, IHtmlWriter writer)
        { }

        /// <summary>
        /// Called before redering control (<see cref="IControl"/>).
        /// </summary>
        /// <param name="control">Target control.</param>
        /// <param name="writer">Output writer.</param>
        protected virtual void BeforeRenderControl(IControl control, IHtmlWriter writer)
        { }

        /// <summary>
        /// Do render phase on <paramref name="control"/>.
        /// </summary>
        /// <param name="control">Target control.</param>
        /// <param name="writer">Output writer.</param>
        protected virtual void DoRenderControl(IControl control, IHtmlWriter writer)
        {
            control.Render(writer);
        }

        /// <summary>
        /// Called after (whole) render phase was completed on <paramref name="control"/>.
        /// </summary>
        /// <param name="control">Target control.</param>
        /// <param name="writer">Output writer.</param>
        protected virtual void AfterRenderControl(IControl control, IHtmlWriter writer)
        { }

        public void DisposeAll()
        {
            foreach (object entry in entries.Keys)
                Dispose(entry);
        }

        public void Dispose(object component)
        {
            // if not registered, continue on processing view.
            if (!entries.ContainsKey(component))
                return;

            ComponentEntryBase entry = entries[component];

            // if not inited, init it.
            if (!entry.IsInited)
                Init(component);

            // if already disposed, continue on processing view.
            if (entry.IsDisposed)
                return;

            IDisposable target = entry.Control as IDisposable;
            if (target != null)
            {
                entry.IsDisposed = true;
                target.Dispose();
            }
        }

        /// <summary>
        /// Called before disposing any component.
        /// </summary>
        /// <param name="component">Target component.</param>
        protected virtual void BeforeDisposeComponent(object component)
        { }

        /// <summary>
        /// Called before disposing control (<see cref="IControl"/>).
        /// </summary>
        /// <param name="component">Target component.</param>
        protected virtual void BeforeDisposeControl(IControl control)
        { }
    }
}
