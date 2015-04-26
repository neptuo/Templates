using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Tokenizers.IO
{
    /// <summary>
    /// Reads content until termination function returns <c>true</c>.
    /// Supports single char escape.
    /// </summary>
    public class PartialReader : IContentReader
    {
        private readonly IContentReader contentReader;
        private readonly Func<char, bool> terminator;
        private readonly char? escape;
        private int startOffset;
        private bool isAfterEscape;
        private bool hasNext;
        private bool isBlankRead;

        public int Position
        {
            get
            {
                if (isBlankRead)
                    return -1;

                return contentReader.Position;
            }
        }

        public char Current
        {
            get
            {
                if (!isBlankRead && hasNext)
                    return contentReader.Current;

                return ContentReader.EndOfInput;
            }
        }

        /// <summary>
        /// Creates new intance that reads content from <paramref name="contentReader"/>.
        /// Reading is terminated when <paramref name="terminator"/> returns <c>true</c>, but
        /// termination can be escaped when <paramref name="escape"/> is just before <paramref name="terminator"/>.
        /// </summary>
        /// <param name="contentReader">Source content reader.</param>
        /// <param name="terminator">Termination function.</param>
        /// <param name="escape">Termination escape char.</param>
        public PartialReader(IContentReader contentReader, Func<char, bool> terminator, char? escape = null)
        {
            Ensure.NotNull(contentReader, "contentReader");
            Ensure.NotNull(terminator, "terminator");
            this.contentReader = contentReader;
            this.terminator = terminator;
            this.escape = escape;
            this.hasNext = true;

            if (contentReader.Position != -1)
                isBlankRead = true;
        }

        public bool Next()
        {
            if (isBlankRead)
            {
                isBlankRead = false;
                return true;
            }

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
            else if (terminator(contentReader.Current))
            {
                if (isAfterEscape)
                    return true;

                return false;
            }
            else
            {
                isAfterEscape = false;
            }

            return true;
        }
    }
}
