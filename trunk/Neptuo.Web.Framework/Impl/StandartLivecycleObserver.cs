using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework
{
    public class StandartLivecycleObserver : ILivecycleObserver
    {
        private List<ILivecycle> objects = new List<ILivecycle>();
        private List<ILivecycle> addedObjects;
        private bool isEvaluating = false;
        private LivecycleState state = LivecycleState.Null;

        public void Add(ILivecycle livecycleObject)
        {
            if (!isEvaluating)
            {
                objects.Add(livecycleObject);
                ProcessMissedEvents(livecycleObject);
            }
            else
                addedObjects.Add(livecycleObject);
        }

        public void Add(object objectToResolve)
        {
            ILivecycle livecycleObject = objectToResolve as ILivecycle;
            if (livecycleObject != null)
                Add(livecycleObject);
        }

        public void OnInit()
        {
            if (state != LivecycleState.Null)
                throw new LivecycleException("OnInit called!");

            OnEvent(objects, l => l.OnInit());
            state = LivecycleState.OnInited;
        }

        public void OnLoad()
        {
            if (state != LivecycleState.OnInited)
                throw new LivecycleException("OnLoad called or OnInit not yet called!");

            OnEvent(objects, l => l.OnLoad());
            state = LivecycleState.OnLoaded;
        }

        public void BeforeRender()
        {
            if (state != LivecycleState.OnLoaded)
                throw new LivecycleException("BeforeRender called or OnLoad not yet called!");

            OnEvent(objects, l => l.BeforeRender());
            state = LivecycleState.Rendered;
        }

        public void BeforeUnLoad()
        {
            if (state != LivecycleState.Rendered)
                throw new LivecycleException("BeforeUnLoad called or BeforeRender not yet called!");

            OnEvent(objects, l => l.BeforeUnLoad());
            state = LivecycleState.UnLoaded;
        }

        private void OnEvent(IEnumerable<ILivecycle> objects, Action<ILivecycle> itemEvent)
        {
            isEvaluating = true;
            addedObjects = new List<ILivecycle>();

            foreach (ILivecycle item in objects)
                itemEvent(item);

            if (addedObjects.Count != 0)
            {
                foreach (ILivecycle livecycleObject in addedObjects)
                    ProcessMissedEvents(livecycleObject);

                IEnumerable<ILivecycle> newObjects = addedObjects.ToList();
                addedObjects = new List<ILivecycle>();
                OnEvent(newObjects, itemEvent);
            }
            isEvaluating = false;
        }

        private void ProcessMissedEvents(ILivecycle livecycleObject)
        {
            if (state == LivecycleState.OnInited)
            {
                livecycleObject.OnInit();
            }
            else if (state == LivecycleState.OnLoaded)
            {
                livecycleObject.OnInit();
                livecycleObject.OnLoad();
            }
            else if (state == LivecycleState.Rendered)
            {
                livecycleObject.OnInit();
                livecycleObject.OnLoad();
                livecycleObject.BeforeRender();
            }
            else if (state == LivecycleState.UnLoaded)
            {
                livecycleObject.OnInit();
                livecycleObject.OnLoad();
                livecycleObject.BeforeUnLoad();
            }
        }
    }
}
