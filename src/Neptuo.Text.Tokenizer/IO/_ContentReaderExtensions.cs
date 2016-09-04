using Neptuo.Text.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Text.IO
{
    public static class _ContentReaderExtensions
    {
        /// <summary>
        /// Reads <paramref name="count"/> characters from <paramref name="contentReader"/>
        /// </summary>
        /// <param name="contentReader">Content source.</param>
        /// <param name="count">Count of characters to read.</param>
        public static void Read(this IContentReader contentReader, int count)
        {
            Ensure.NotNull(contentReader, "contentReader");
            Ensure.Positive(count, "count");

            for (int i = 0; i < count; i++)
                contentReader.Next();
        }

        /// <summary>
        /// Reads remaing content from <paramref name="contentReader"/>.
        /// CURRENT character is IGNORED (methods starts reading next character).
        /// </summary>
        /// <param name="contentReader">Content source.</param>
        /// <returns>Remaing content from <paramref name="contentReader"/>.</returns>
        public static StringBuilder NextToEnd(this IContentReader contentReader)
        {
            Ensure.NotNull(contentReader, "contentReader");

            StringBuilder result = new StringBuilder();
            while (contentReader.Next())
            {
                result.Append(contentReader.Current);
            }

            return result;
        }

        /// <summary>
        /// Reads remaing content from <paramref name="contentReader"/>.
        /// CURRENT character is USED (methods starts comparing current character).
        /// </summary>
        /// <param name="contentReader">Content source.</param>
        /// <returns>Remaing content from <paramref name="contentReader"/>.</returns>
        public static StringBuilder CurrentToEnd(this IContentReader contentReader)
        {
            Ensure.NotNull(contentReader, "contentReader");
            StringBuilder result = new StringBuilder();

            if (contentReader.Current == ContentReader.EndOfInput)
                return result;

            do
            {
                result.Append(contentReader.Current);
            } while (contentReader.Next());

            return result;
        }

        /// <summary>
        /// Returns <c>true</c> if <see cref="IContentReader.Current"/> of <paramref name="contentReader"/> is <see cref="ContentReader.EndOfInput"/>; <c>false</c> otherwise.
        /// </summary>
        /// <param name="contentReader">Content source.</param>
        /// <returns><c>true</c> if <see cref="IContentReader.Current"/> of <paramref name="contentReader"/> is <see cref="ContentReader.EndOfInput"/>; <c>false</c> otherwise.</returns>
        public static bool IsCurrentEndOfInput(this IContentReader contentReader)
        {
            Ensure.NotNull(contentReader, "contentReader");
            return contentReader.Current == ContentReader.EndOfInput;
        }

        /// <summary>
        /// Returns <c>true</c> if <paramref name="IContentReader.Current"/> is contained in <paramref name="items"/>.
        /// </summary>
        /// <param name="items">The sequence of chars to test.</param>
        /// <returns></returns>
        public static bool IsCurrentContained(this IContentReader contentReader, params char[] items)
        {
            Ensure.NotNull(contentReader, "contentReader");
            Ensure.NotNull(items, "items");
            return items.Contains(contentReader.Current);
        }
    }
}
