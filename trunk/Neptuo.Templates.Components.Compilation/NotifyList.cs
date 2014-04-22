using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation
{
    internal class NotifyList<T> : ICollection<T>
    {
        private List<T> storage;

        public event Action<T> OnAddItem;
        public event Func<T, bool> OnRemoveItem;
        public event Action OnClear;

        public NotifyList()
        {
            storage = new List<T>();
        }

        public NotifyList(Action<T> onAddItem, Func<T, bool> onRemoveItem, Action onClear)
            : this()
        {
            OnAddItem += onAddItem;
            OnRemoveItem += onRemoveItem;
            onClear += onClear;
        }

        public NotifyList(IEnumerable<T> items)
        {
            storage = new List<T>(items);
        }

        public void Add(T item)
        {
            storage.Add(item);

            if (OnAddItem != null)
                OnAddItem(item);
        }

        public void Clear()
        {
            storage.Clear();

            if (OnClear != null)
                OnClear();
        }

        public bool Contains(T item)
        {
            return storage.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            storage.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return storage.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            bool result = storage.Remove(item);
            if (result && OnRemoveItem != null)
                OnRemoveItem(item);
            
            return result;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return storage.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return storage.GetEnumerator();
        }
    }
}
