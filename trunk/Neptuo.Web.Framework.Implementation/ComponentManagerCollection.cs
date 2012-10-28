using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework
{
    public class ComponentManagerCollection<T> : ICollection<T>
    {
        protected IComponentManager componentManager;
        protected string propertyName;
        protected object owner;
        protected ICollection<object> dataStorage;

        public ComponentManagerCollection(IComponentManager componentManager, string propertyName, object owner)
        {
            this.componentManager = componentManager;
            this.propertyName = propertyName;
            this.owner = owner;
            this.dataStorage = componentManager.GetComponents(owner, propertyName);
        }

        public void Add(T item)
        {
            Add(item, null);
        }

        public void Add(T item, Action propertyBinder)
        {
            componentManager.AddComponent(owner, propertyName, item, propertyBinder);
        }

        public void Clear()
        {
            dataStorage.Clear();
        }

        public bool Contains(T item)
        {
            return dataStorage.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException("CopyTo not supported!");
            //dataStorage.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return dataStorage.Count; }
        }

        public bool IsReadOnly
        {
            get { return dataStorage.IsReadOnly; }
        }

        public bool Remove(T item)
        {
            return componentManager.RemoveComponent(owner, propertyName, item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return dataStorage.OfType<T>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return dataStorage.GetEnumerator();
        }
    }
}
