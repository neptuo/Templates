﻿using System;
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
        private int position;

        public int Position
        {
            get
            {
                if(value == null)
                    return -1;

                if (position >= value.Length)
                    return position - 1;

                return position;
            }
        }

        public char Current
        {
            get
            {
                if (position < 0)
                    return NullChar;

                if (value == null)
                    return NullChar;

                if (position >= value.Length)
                    return NullChar;

                return value[position];
            }
        }

        /// <summary>
        /// Creates new instance which reads characters from <paramref name="value"/>.
        /// </summary>
        /// <param name="value">Content characters.</param>
        public StringContentReader(string value)
        {
            this.value = value;
            position = -1;
        }

        public bool Next()
        {
            if (value == null || position >= value.Length)
                return false;

            position++;
            return position < value.Length;
        }
    }
}
