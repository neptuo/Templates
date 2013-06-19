using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Composition.Data
{
    public class ContentStorage
    {
        Stack<ContentStorageItem> storage = new Stack<ContentStorageItem>();

        public ContentStorage()
        {
            storage.Push(new ContentStorageItem());
        }

        public void Push(ContentStorageItem item)
        {
            storage.Push(item);
        }

        public void Pop()
        {
            storage.Pop();
        }

        public ContentStorageItem Peek()
        {
            return storage.Peek();
        }

        public ContentStorageItem CreateItem()
        {
            return new ContentStorageItem();
        }
    }

    public class ContentStorageItem
    {
        private Dictionary<string, ContentControl> storage = new Dictionary<string, ContentControl>();

        public void Add(string key, ContentControl content)
        {
            storage.Add(key, content);
        }

        public void Remove(string key)
        {
            storage.Remove(key);
        }

        public ContentControl Get(string key)
        {
            return storage[key];
        }

        public bool ContainsKey(string key)
        {
            return storage.ContainsKey(key);
        }
    }
}
