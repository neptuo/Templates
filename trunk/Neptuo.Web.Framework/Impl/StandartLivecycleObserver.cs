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

        public void Register(object parent, object control)
        {
            Register(parent, control, null);
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

        public void RegisterObserver(object control, IObserver observer, string attributeName, Func<object> observerInitializer)
        {
            if(!entries.ContainsKey(control))
                throw new LivecycleException("Control is not registered, unnable to register observer");

            entries[control].Observers.Add(new ObserverInfo(attributeName, observer, observerInitializer));
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
                foreach (ObserverInfo info in entry.Observers)
                {
                    info.AttributeValue = info.Initializer();

                    ObserverEventArgs args = CreateObserverEventArgs(entry.Control, info);
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
                foreach (ObserverInfo info in entry.Observers)
                {
                    ObserverEventArgs args = CreateObserverEventArgs(entry.Control, info);
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

        private ObserverEventArgs CreateObserverEventArgs(object target, ObserverInfo observerInfo)
        {
            return new ObserverEventArgs
            {
                AttributeName = observerInfo.AttributeName,
                AttributeValue = observerInfo.AttributeValue,
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
        public string AttributeName { get; set; }

        public object AttributeValue { get; set; }

        public Func<object> Initializer { get; set; }

        public IObserver Observer { get; set; }

        public ObserverInfo(string attributeName, IObserver observer, Func<object> initializer)
        {
            AttributeName = attributeName;
            Observer = observer;
            Initializer = initializer;
        }
    }
}
