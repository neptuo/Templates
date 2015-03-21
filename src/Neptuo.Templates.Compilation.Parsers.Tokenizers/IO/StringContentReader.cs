using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Tokenizers.IO
{
    /// <summary>
    /// String content reader.
    /// Reads characters from string value.
    /// </summary>
    public class StringContentReader : IContentReader
    {
        /// <summary>
        /// Default character value when non from input is available.
        /// </summary>
        public const char NullChar = '\0';

        private readonly string value;

        public int Position { get; private set; }

        public char Current
        {
            get
            {
                if (Position < 0)
                    return NullChar;

                if (value == null)
                    return NullChar;

                if (Position >= value.Length)
                    return NullChar;

                return value[Position];
            }
        }

        /// <summary>
        /// Creates new instance which reads characters from <paramref name="value"/>.
        /// </summary>
        /// <param name="value">Content characters.</param>
        public StringContentReader(string value)
        {
            this.value = value;
            Position = -1;
        }

        public bool Next()
        {
            if (value == null || Position >= value.Length)
                return false;

            Position++;
            return Position < value.Length;
        }
    }
}
