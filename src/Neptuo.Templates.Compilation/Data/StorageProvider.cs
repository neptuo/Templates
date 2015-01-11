using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.Data
{
    /// <summary>
    /// Provides access to context storage.
    /// </summary>
    public class StorageProvider
    {
        private Dictionary<string, object> storage = new Dictionary<string, object>();

        /// <summary>
        /// Creates named storage or returns already created one.
        /// </summary>
        /// <typeparam name="T">Type of storege.</typeparam>
        /// <param name="name">Storage name.</param>
        /// <returns>Creates named storage or returns already created one.</returns>
        public T Create<T>(string name)
            where T : class, new()
        {
            if (!storage.ContainsKey(name))
                storage[name] = new T();
                
            return (T)storage[name];
        }
    }
}
