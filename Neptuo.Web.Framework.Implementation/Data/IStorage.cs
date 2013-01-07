using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Data
{
    public interface IStorage<TKey, TValue>
    {
        void Add(TKey key, TValue value);
        TValue Get(TKey key);
        bool TryGetValue(TKey key, out TValue value);
    }

    public interface IStorage : IStorage<string, object> 
    { }
}
