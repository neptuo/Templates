using Neptuo.Templates.Compilation.Parsers.Tokenizers.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Tokenizers.IO
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
    }
}
