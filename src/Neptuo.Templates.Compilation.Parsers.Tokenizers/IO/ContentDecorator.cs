using Neptuo.ComponentModel;
using Neptuo.ComponentModel.TextOffsets;
using Neptuo.Templates.Compilation.Parsers.Tokenizers.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Tokenizers.IO
{
    /// <summary>
    /// Extended <see cref="IContentReader"/> providing content and line info.
    /// Also supports re-reading of content.
    /// </summary>
    public class ContentDecorator : IContentReader
    {
        private readonly Dictionary<int, int> lineLenghts = new Dictionary<int, int>();
        private readonly IContentReader contentReader;
        private IContentReader currentReader;

        private IContentReader CurrentReader
        {
            get
            {
                if (currentReader == null)
                    return contentReader;

                return currentReader;
            }
        }

        private int currentStartIndex;
        private int currentLength;

        private int currentStartLineIndex;
        private int currentStartColumnIndex;

        private int currentLineIndex;
        private int currentColumnIndex;

        private StringBuilder currentContent;

        /// <summary>
        /// Creates new instance that wraps content from <paramref name="contentReader"/>.
        /// </summary>
        /// <param name="contentReader">Inner content provider.</param>
        public ContentDecorator(IContentReader contentReader)
        {
            Ensure.NotNull(contentReader, "contentReader");
            this.contentReader = contentReader;
            this.currentContent = new StringBuilder();
            UpdateContentInfoState();
        }

        #region IContentReader

        public int Position
        {
            get { return CurrentReader.Position; }
        }

        public char Current
        {
            get { return CurrentReader.Current; }
        }

        public bool Next()
        {
            bool hasNext = false;
            if (currentReader != null)
            {
                hasNext = currentReader.Next();
                if (!hasNext)
                    currentReader = null;
            }

            if (!hasNext)
                hasNext = contentReader.Next();

            if (hasNext)
            {
                UpdateContentInfoState();
                return true;
            }

            return false;
        }

        #endregion

        private void UpdateContentInfoState()
        {
            currentLength++;
            currentContent.Append(Current);

            if (Current == '\n')
            {
                lineLenghts[currentLineIndex] = currentColumnIndex + 1;

                currentLineIndex++;
                currentColumnIndex = 0;

            }
            else
            {
                currentColumnIndex++;
            }
        }

        /// <summary>
        /// Returns current 'token' content info.
        /// Values are reseted calling <see cref="ContentDecorator.ResetCurrentInfo"/>.
        /// </summary>
        /// <returns>Current 'token' content info.</returns>
        public IContentRangeInfo CurrentContentInfo()
        {
            return new DefaultContentRangeInfo(currentStartIndex, currentLength);
        }

        /// <summary>
        /// Returns current 'token' line info.
        /// Values are reseted calling <see cref="ContentDecorator.ResetCurrentInfo"/>.
        /// </summary>
        /// <returns>Current 'token' line info.</returns>
        public ILineRangeInfo CurrentLineInfo()
        {
            return new DefaultLineRangeInfo(currentStartColumnIndex, currentStartLineIndex, currentColumnIndex, currentLineIndex);
        }

        /// <summary>
        /// Returns current 'token' text value.
        /// </summary>
        /// <returns>Current 'token' text value.</returns>
        public string CurrentContent()
        {
            return currentContent.ToString();
        }

        /// <summary>
        /// Resets current 'token' line and content info.
        /// Moves current position as start position and that increments from this position.
        /// </summary>
        public void ResetCurrentInfo()
        {
            currentStartIndex = currentStartIndex + currentLength;
            currentLength = 0;
            currentStartColumnIndex = currentColumnIndex;
            currentStartLineIndex = currentLineIndex;

            currentContent.Clear();
        }

        /// <summary>
        /// Moves cursor in content reading to <paramref name="stepsToGoBack"/> chars back.
        /// Only reset inside current 'token' is supported.
        /// </summary>
        /// <param name="stepsToGoBack">Number of characters to go back.</param>
        /// <returns>If reset was (<c>true</c>) or was'nt (<c>false</c>) successfull.</returns>
        public bool ResetCurrentPosition(int stepsToGoBack)
        {
            // Test if we can go back.
            if (currentContent.Length < stepsToGoBack)
                return false;

            string text = currentContent.ToString();
            string toResetText = text.Substring(text.Length - stepsToGoBack);
            string newCurrent = text.Substring(0, text.Length - stepsToGoBack);
            char firstChar = newCurrent.Length > 0 ? newCurrent[newCurrent.Length - 1] : ContentReader.EndOfInput;

            // Update current 'token'.
            currentContent.Clear();
            currentContent.Append(newCurrent);

            // Prepare temporal reader.
            currentLength = currentContent.Length;
            currentReader = new StringReader(firstChar + toResetText, currentStartIndex);

            // Update line info.
            foreach (char item in toResetText.Reverse())
            {
                if (item == '\n')
                {
                    currentLineIndex--;
                    currentColumnIndex = lineLenghts[currentLineIndex] - 1;
                }
                else
                {
                    currentColumnIndex--;
                }
            }

            return true;
        }
    }
}
