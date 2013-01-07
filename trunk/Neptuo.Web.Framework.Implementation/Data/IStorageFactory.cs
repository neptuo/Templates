using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Data
{
    public interface IStorageFactory
    {
        IStorage<TKey, TValue> GetStorage<TKey, TValue>(string name, StorageLifecycle livecycle);
        IStorage GetStorage(string name, StorageLifecycle livecycle);
    }
}
