using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    /// <summary>
    /// Common extensions for <see cref="CodePropertyCollection"/>.
    /// </summary>
    public static class _CodePropertyCollectionExtensions
    {
        /// <summary>
        /// Adds all items from <paramref name="codeProperties"/> to <paramref name="collection"/>.
        /// </summary>
        /// <param name="collection">Target collection of code properties.</param>
        /// <param name="codeProperties">Items to add to the <paramref name="collection"/>.</param>
        /// <returns><paramref name="collection"/>.</returns>
        public static CodePropertyCollection AddRange(this CodePropertyCollection collection, IEnumerable<ICodeProperty> codeProperties)
        {
            Ensure.NotNull(collection, "collection");
            Ensure.NotNull(codeProperties, "codeProperties");

            foreach (ICodeProperty codeProperty in codeProperties)
                collection.Add(codeProperty);

            return collection;
        }
    }
}
