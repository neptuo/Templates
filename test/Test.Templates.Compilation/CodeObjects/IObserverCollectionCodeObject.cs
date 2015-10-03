using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    /// <summary>
    /// Defines code object with collection of observers.
    /// </summary>
    public interface IObserverCollectionCodeObject
    {
        /// <summary>
        /// Adds <paramref name="observer"/> to the collection of observers.
        /// </summary>
        /// <param name="observer">The observer to add.</param>
        /// <returns>Self (for fluency).</returns>
        IObserverCollectionCodeObject AddObserver(ICodeObject observer);

        /// <summary>
        /// Returns enumeration of observers.
        /// </summary>
        /// <returns>Enumeration of observers.</returns>
        IEnumerable<ICodeObject> EnumerateObservers();
    }
}
