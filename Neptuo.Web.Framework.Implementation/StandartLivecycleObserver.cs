using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework
{
    public class StandartLivecycleObserver : ILivecycleObserver
    {
        private Dictionary<object, LivecycleEntry> entries = new Dictionary<object, LivecycleEntry>();
        private Dictionary<object, List<LivecycleEntry>> children = new Dictionary<object, List<LivecycleEntry>>();

        private object root = new object();

        public StandartLivecycleObserver()
        {
            children.Add(root, new List<LivecycleEntry>());
        }

        public IEnumerable<object> GetControls()
        {
            return entries.Values;
        }

        public void Register(object parent, object control, Action propertyBinder)
        {
            if (parent == null)
                parent = root;

            if (!children.ContainsKey(parent))
                throw new LivecycleException("Parent is not registered!");

            if (!(control is IControl) && !(control is IViewPage))
                return;

            LivecycleEntry entry = new LivecycleEntry
            {
                Control = control,
                Parent = parent,
                ArePropertiesBound = propertyBinder == null,
                PropertyBinder = propertyBinder
            };

            children[parent].Add(entry);

            if(!children.ContainsKey(control))
                children.Add(control, new List<LivecycleEntry>());

            entries.Add(control, entry);
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

            LivecycleEntry entry = entries[control];
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
                ObserverEventArgs args = CreateObserverEventArgs(target);
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

            LivecycleEntry entry = entries[control];

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
                ObserverEventArgs args = CreateObserverEventArgs(target);
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

        public void Dispose(object control)
        {
            if (!entries.ContainsKey(control))
                throw new LivecycleException("Not registered control!");

            LivecycleEntry entry = entries[control];

            if (!entry.IsInited)
                Init(control);

            if (entry.IsDisposed)
                throw new LivecycleException("Control is already disposed!");

            IDisposable target = entry.Control as IDisposable;
            if (target != null)
            {
                target.Dispose();
                entry.IsDisposed = true;
            }
        }

        private ObserverEventArgs CreateObserverEventArgs(IControl target)
        {
            return new ObserverEventArgs
            {
                Target = target
            };
        }
    }

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
