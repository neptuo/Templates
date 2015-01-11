using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    public static class _CodeObjectListExtensions
    {
        /// <summary>
        /// Adds <see cref="PlainValueCodeObject"/> with value <paramref name="value"/> to the <paramref name="list"/>.
        /// </summary>
        /// <param name="list">Target list of code objects.</param>
        /// <param name="value">Plain value to add.</param>
        /// <returns><paramref name="list"/>.</returns>
        public static CodeObjectList AddPlainValue(this CodeObjectList list, object value)
        {
            Guard.NotNull(list, "list");
            list.Add(new PlainValueCodeObject(value));
            return list;
        }

        /// <summary>
        /// Adds <see cref="CommentCodeObject"/> with value <paramref name="commentText"/> to the <paramref name="list"/>.
        /// </summary>
        /// <param name="list">Target list of code objects.</param>
        /// <param name="commentText">Comment text.</param>
        /// <returns><paramref name="list"/>.</returns>
        public static CodeObjectList AddComment(this CodeObjectList list, string commentText)
        {
            Guard.NotNull(list, "list");
            list.Add(new CommentCodeObject(commentText));
            return list;
        }
    }
}
