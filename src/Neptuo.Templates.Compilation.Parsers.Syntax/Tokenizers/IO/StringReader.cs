using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers.IO
{
    /// <summary>
    /// String content reader.
    /// Reads characters from string value.
    /// </summary>
    public class StringReader : IContentReader
    {

        private readonly string value;
        private readonly int offset;
        private int position;

        public int Position
        {
            get
            {
                if(value == null)
                    return -1;

                if (position >= value.Length)
                    return position - 1;

                return offset + position;
            }
        }

        public char Current
        {
            get
            {
                if (value == null)
                    return ContentReader.EndOfInput;

                if (position >= value.Length)
                    return ContentReader.EndOfInput;

                return value[position];
            }
        }

        /// <summary>
        /// Creates new instance which reads characters from <paramref name="value"/>.
        /// </summary>
        /// <param name="value">Content characters.</param>
        public StringReader(string value)
            : this(value, 0)
        { }

        /// <summary>
        /// Creates new instance which reads characters from <paramref name="value"/>
        /// and Position is prefixed with <paramref name="offset"/>.
        /// </summary>
        /// <param name="value">Content characters.</param>
        /// <param name="offset">Start position offset.</param>
        public StringReader(string value, int offset)
        {
            this.value = value;
            this.offset = offset;
            position = 0;
        }

        public bool Next()
        {
            if (value == null || position >= value.Length)
                return false;

            position++;
            return position < value.Length;
        }

        public override string ToString()
        {
            string currentValue;
            if (value != null && value.Length > 10)
                currentValue = value.Substring(Math.Max(0, position - 5), 10);
            else
                currentValue = value;

            return String.Format("('{0}' at {1}) of '{2}'", Current, position, currentValue);
        }
    }
}
