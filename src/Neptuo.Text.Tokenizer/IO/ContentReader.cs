using Neptuo.Activators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Text.IO
{
    /// <summary>
    /// Helper class for creating different content readers.
    /// </summary>
    public static class ContentReader
    {
        /// <summary>
        /// Default character value when non from input is available.
        /// </summary>
        public const char EndOfInput = '\0';

        public static IContentReader String(string value)
        {
            return new StringReader(value);
        }

        public static IContentReader Partial(IContentReader reader, Func<char, bool> terminator)
        {
            return new PartialReader(reader, terminator);
        }
        
        /// <summary>
        /// Creates new intance that reads content from <paramref name="contentReader"/>.
        /// Reading is terminated when one of the <paramref name="terminators"/> is found, but
        /// can be escaped when <c>\</c> is just before this char.
        /// </summary>
        /// <param name="contentReader">Source content reader.</param>
        /// <param name="terminators">Enumeration of possible terminators.</param>
        /// <returns>New intance of partial reader.</returns>
        public static IContentReader Partial(IContentReader reader, params char[] terminators)
        {
            return new PartialReader(reader, terminators.Contains, '\\');
        }

        public static IFactory<IContentReader> Factory(IContentReader reader)
        {
            return new ContentFactory(reader);
        }
    }
}
