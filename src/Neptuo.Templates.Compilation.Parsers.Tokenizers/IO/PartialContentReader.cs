using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Tokenizers.IO
{
    /// <summary>
    /// Reads content until specified character is found.
    /// Supports single char escape.
    /// </summary>
    public class PartialContentReader : IContentReader
    {
        private readonly IContentReader contentReader;
        private readonly char terminator;
        private readonly char escape;
        private bool isAfterEscape;
        private bool hasNext;

        public int Position
        {
            get { return contentReader.Position; }
        }

        public char Current
        {
            get
            {
                if(hasNext)
                    return contentReader.Current;

                return StringContentReader.NullChar;
            }
        }

        /// <summary>
        /// Creates new intance that reads content from <paramref name="contentReader"/>.
        /// Reading is terminated when <paramref name="terminator"/> is found, but
        /// termination can be escaped when <paramref name="escape"/> is just before <paramref name="terminator"/>.
        /// </summary>
        /// <param name="contentReader">Source content reader.</param>
        /// <param name="terminator">Termination char.</param>
        /// <param name="escape">Termination escape char.</param>
        public PartialContentReader(IContentReader contentReader, char terminator, char escape = '\\')
        {
            Ensure.NotNull(contentReader, "contentReader");
            this.contentReader = contentReader;
            this.terminator = terminator;
            this.escape = escape;
            this.hasNext = true;
        }

        public bool Next()
        {
            if (hasNext)
            {
                hasNext = contentReader.Next();
                if (hasNext)
                    hasNext = UpdateCurrentState();
            }

            return hasNext;
        }

        private bool UpdateCurrentState()
        {
            if (contentReader.Current == escape)
            {
                isAfterEscape = !isAfterEscape;
            }
            else if (contentReader.Current == terminator)
            {
                if (isAfterEscape)
                    return true;

                return false;
            }

            isAfterEscape = false;
            return true;
        }
    }
}
