using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Mvc.Utils
{
    /// <summary>
    /// Stack storage.
    /// </summary>
    public class StackStorage
    {
        Dictionary<string, Stack<object>> storage = new Dictionary<string, Stack<object>>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object this[string key]
        {
            get { return Peek(key); }
        }

        /// <summary>
        /// Returns the object at the top of the stack with <paramref name="key"/> without removing it.
        /// </summary>
        /// <param name="key">Stack identifier.</param>
        /// <returns>Object from top of stack.</returns>
        public object Peek(string key)
        {
            if (storage.ContainsKey(key))
                return storage[key].Peek();

            return null;
        }

        /// <summary>
        /// Type save version of <see cref="Peek"/>.
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <param name="key">Stack identifier.</param>
        /// <returns>Object from top of stack.</returns>
        public T Peek<T>(string key) where T : class
        {
            return Peek(key) as T;
        }

        /// <summary>
        /// Removes and returns the object at the top of the stack with <paramref name="key"/>.
        /// </summary>
        /// <param name="key">Stack identifier.</param>
        /// <returns>Object from top of stack.</returns>
        public object Pop(string key)
        {
            if (storage.ContainsKey(key))
                return storage[key].Pop();

            return null;
        }

        /// <summary>
        /// Type save version of <see cref="Pop"/>.
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <param name="key">Stack identifier.</param>
        /// <returns>Object from top of stack.</returns>
        public T Pop<T>(string key) where T : class
        {
            return Pop(key) as T;
        }

        /// <summary>
        /// Inserts an object at the top of the stack with <paramref name="key"/>.
        /// </summary>
        /// <param name="key">Stack identifier.</param>
        /// <param name="data">New item.</param>
        public void Push(string key, object data)
        {
            if (!storage.ContainsKey(key))
                storage.Add(key, new Stack<object>());

            storage[key].Push(data);
        }
    }
}
