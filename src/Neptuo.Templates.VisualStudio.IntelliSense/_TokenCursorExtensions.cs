using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.VisualStudio.IntelliSense
{
    /// <summary>
    /// Common extensions for <see cref="TokenCursor"/>.
    /// </summary>
    public static class _TokenCursorExtensions
    {
        /// <summary>
        /// Creates next cursor or throws <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <param name="cursor">The cursor to create next from.</param>
        /// <returns>The next cursor from <paramref name="cursor"/>.</returns>
        public static TokenCursor Next(this TokenCursor cursor)
        {
            Ensure.NotNull(cursor, "cursor");

            TokenCursor next;
            if (cursor.TryNext(out next))
                return next;

            throw Ensure.Exception.InvalidOperation("Unnable to create next token cursor.");
        }

        /// <summary>
        /// Creates previous cursor or throws <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <param name="cursor">The cursor to create previous from.</param>
        /// <returns>The previous cursor from <paramref name="cursor"/>.</returns>
        public static TokenCursor Prev(this TokenCursor cursor)
        {
            Ensure.NotNull(cursor, "cursor");

            TokenCursor prev;
            if (cursor.TryPrev(out prev))
                return prev;

            throw Ensure.Exception.InvalidOperation("Unnable to create previous token cursor.");
        }
    }
}
