using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeObjects.Features
{
    /// <summary>
    /// Default implementation of <see cref="IObserverCollectionCodeObject"/>.
    /// </summary>
    public class ObserverCollectionCodeObject : IObserverCollectionCodeObject
    {
        private readonly List<ICodeObject> storage = new List<ICodeObject>();

        public IObserverCollectionCodeObject AddObserver(ICodeObject observer)
        {
            Ensure.NotNull(observer, "observer");
            storage.Add(observer);
            return this;
        }

        public IEnumerable<ICodeObject> EnumerateObservers()
        {
            return storage;
        }
    }
}
