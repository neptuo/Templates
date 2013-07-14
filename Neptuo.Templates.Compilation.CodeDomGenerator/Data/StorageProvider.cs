using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.Data
{
    public class StorageProvider
    {
        private Dictionary<string, object> storage = new Dictionary<string, object>();

        public T Create<T>(string name)
            where T : class, new()
        {
            if (!storage.ContainsKey(name))
                storage[name] = new T();
                
            return (T)storage[name];
        }
    }
}
