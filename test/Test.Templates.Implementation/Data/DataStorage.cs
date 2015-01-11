using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.Templates.Data
{
    public class DataStorage
    {
        public Stack<object> storage = new Stack<object>();

        public DataStorage(object data)
        {
            Push(data);
        }

        public void Push(object data)
        {
            storage.Push(data);
        }

        public object Pop()
        {
            return storage.Pop();
        }

        public object Peek()
        {
            return storage.Peek();
        }
    }
}
