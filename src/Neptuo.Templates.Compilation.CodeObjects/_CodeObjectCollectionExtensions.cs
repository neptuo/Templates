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


        /// <summary>
        /// Adds <see cref="LiteralCodeObject"/> with value <paramref name="value"/> to the <paramref name="list"/>.
        /// </summary>
        /// <param name="list">Target list of code objects.</param>
        /// <param name="value">Plain value to add.</param>
        /// <returns><paramref name="list"/>.</returns>
        public static CodeObjectCollection AddPlainValue(this CodeObjectCollection list, object value)
        {
            Ensure.NotNull(list, "list");
            list.Add(new LiteralCodeObject(value));
            return list;
        }

        /// <summary>
        /// Adds <see cref="CommentCodeObject"/> with value <paramref name="commentText"/> to the <paramref name="list"/>.
        /// </summary>
        /// <param name="list">Target list of code objects.</param>
        /// <param name="commentText">Comment text.</param>
        /// <returns><paramref name="list"/>.</returns>
        public static CodeObjectCollection AddComment(this CodeObjectCollection list, string commentText)
        {
            Ensure.NotNull(list, "list");
            list.Add(new CommentCodeObject(commentText));
            return list;
        }
    }
}
