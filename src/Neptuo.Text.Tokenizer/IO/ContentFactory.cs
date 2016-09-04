﻿using Neptuo.Activators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Text.IO
{
    /// <summary>
    /// Factory for content readers.
    /// Caches content from single content reader and provides ability to read this content.
    /// </summary>
    public class ContentFactory : IFactory<IContentReader>
    {
        private readonly IContentReader contentReader;
        private readonly StringBuilder cache;

        public ContentFactory(IContentReader contentReader)
        {
            Ensure.NotNull(contentReader, "contentReader");
            this.contentReader = contentReader;
            this.cache = new StringBuilder();
            cache.Append(contentReader.Current);
        }

        /// <summary>
        /// Creates instance of content reader.
        /// </summary>
        /// <returns>Instance of content reader.</returns>
        public IContentReader Create()
        {
            return new ContentFactoryReader(contentReader, cache);
        }

        /// <summary>
        /// Internal implementation of single content reader.
        /// </summary>
        private class ContentFactoryReader : IContentReader
        {
            private readonly IContentReader contentReader;
            private readonly StringBuilder cache;
            private int position;
            private int positionOffset;

            public int Position
            {
                get { return position; }
            }

            public char Current
            {
                get
                {
                    if(position < 0)
                        return ContentReader.EndOfInput;

                    if (position < cache.Length)
                        return cache[position];

                    return contentReader.Current;
                }
            }

            public ContentFactoryReader(IContentReader contentReader, StringBuilder cache)
            {
                this.contentReader = contentReader;
                this.cache = cache;
                position = 0;
                positionOffset = contentReader.Position;
            }

            public bool Next()
            {
                position++;
                if(position <= contentReader.Position - positionOffset)
                    return true;

                bool hasValue = contentReader.Next();
                if (hasValue)
                {
                    cache.Append(contentReader.Current);
                    if (position != contentReader.Position)
                        positionOffset = contentReader.Position - position;
                }

                return hasValue;
            }
        }
    }
}
