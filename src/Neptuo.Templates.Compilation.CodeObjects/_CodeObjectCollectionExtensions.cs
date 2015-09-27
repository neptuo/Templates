using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    /// <summary>
    /// Common extensions for <see cref="CodeObjectCollection"/>.
    /// </summary>
    public static class _CodeObjectCollectionExtensions
    {
        /// <summary>
        /// Adds all items from <paramref name="codeObjects"/> to <paramref name="collection"/>.
        /// </summary>
        /// <param name="collection">Target collection of code objects.</param>
        /// <param name="codeObjects">Items to add to the <paramref name="collection"/>.</param>
        /// <returns><paramref name="collection"/>.</returns>
        public static CodeObjectCollection AddRange(this CodeObjectCollection collection, IEnumerable<ICodeObject> codeObjects)
        {
            Ensure.NotNull(collection, "collection");
            Ensure.NotNull(codeObjects, "codeObjects");

            foreach (ICodeObject codeObject in codeObjects)
                collection.Add(codeObject);

            return collection;
        }
    }
}
