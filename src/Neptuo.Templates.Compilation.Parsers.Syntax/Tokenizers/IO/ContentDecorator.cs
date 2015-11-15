using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers.ComponentModel;
using Neptuo.Text.Positions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers.IO
{
    /// <summary>
    /// Extended <see cref="IContentReader"/> providing content and line info.
    /// Also supports re-reading of content.
    /// </summary>
    public class ContentDecorator : IContentReader, ICurrentInfoAware
    {
        private readonly Dictionary<int, int> lineLenghts = new Dictionary<int, int>();
        private readonly IContentReader contentReader;
        private readonly int positionOffset;
        private Stack<IContentReader> currentReader;

        private IContentReader CurrentReader
        {
            get
            {
                if (currentReader.Count == 0)
                    return contentReader;

                return currentReader.Peek();
            }
        }

        private int currentStartIndex;
        private int currentLength;

        private int currentStartLineIndex;
        private int currentStartColumnIndex;

        private int currentLineIndex;
        private int currentColumnIndex;

        private StringBuilder currentContent;

        public int LineIndex
        {
            get { return currentLineIndex; }
        }

        public int ColumnIndex
        {
            get { return currentColumnIndex; }
        }

        /// <summary>
        /// Creates new instance that wraps content from <paramref name="contentReader"/>.
        /// </summary>
        /// <param name="contentReader">Inner content provider.</param>
        public ContentDecorator(IContentReader contentReader)
        {
            Ensure.NotNull(contentReader, "contentReader");
            this.contentReader = contentReader;
            this.currentContent = new StringBuilder();
            this.currentReader = new Stack<IContentReader>();
            UpdateContentInfoState();
        }

        /// <summary>
        /// Creates new instance that wraps content from <paramref name="contentReader"/>
        /// with line info offset.
        /// </summary>
        /// <param name="contentReader">Inner content provider.</param>
        /// <param name="lineInfo">Line info offset.</param>
        public ContentDecorator(IContentReader contentReader, int positionOffset, int lineOffset, int columnOffset)
        {
            Ensure.NotNull(contentReader, "contentReader");
            Ensure.PositiveOrZero(positionOffset, "positionOffset");
            Ensure.PositiveOrZero(lineOffset, "lineOffset");
            Ensure.PositiveOrZero(columnOffset, "columnOffset");
            this.contentReader = contentReader;
            this.currentContent = new StringBuilder();
            this.currentReader = new Stack<IContentReader>();
            this.positionOffset = positionOffset;
            currentStartIndex = positionOffset;
            currentLineIndex = currentStartLineIndex = lineOffset;
            currentColumnIndex = currentStartColumnIndex = columnOffset;
            UpdateContentInfoState();
        }

        #region IContentReader

        public int Position
        {
            get { return CurrentReader.Position + positionOffset; }
        }

        public char Current
        {
            get { return CurrentReader.Current; }
        }

        public bool Next()
        {
            bool hasNext = false;
            while (!hasNext && currentReader.Count != 0)
            {
                hasNext = currentReader.Peek().Next();
                if (!hasNext)
                    currentReader.Pop();
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
        /// <param name="length">Optional override of length.</param>
        /// <returns>Current 'token' content info.</returns>
        public ITextSpan CurrentContentInfo(int? length = null)
        {
            return new DefaultTextSpan(currentStartIndex, length ?? currentLength);
        }

        /// <summary>
        /// Returns current 'token' line info.
        /// Values are reseted calling <see cref="ContentDecorator.ResetCurrentInfo"/>.
        /// </summary>
        /// <returns>Current 'token' line info.</returns>
        public IDocumentSpan CurrentLineInfo()
        {
            return new DefaultDocumentSpan(currentStartLineIndex, currentStartColumnIndex, currentLineIndex, currentColumnIndex);
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
        /// Returns <c>true</c> if current state enables to take <paramref name="stepsToGoBack"/> steps back; <c>false</c> otherwise.
        /// </summary>
        /// <param name="stepsToGoBack">Number of characters to go back.</param>
        /// <returns><c>true</c> if current state enables to take <paramref name="stepsToGoBack"/> steps back; <c>false</c> otherwise.</returns>
        public bool CanResetCurrentPosition(int stepsToGoBack)
        {
            if (currentContent.Length < stepsToGoBack || stepsToGoBack < 0)
                return false;

            return true;
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
            if (!CanResetCurrentPosition(stepsToGoBack))
                return false;

            // Trying to move by 0 is totally ok.
            if (stepsToGoBack == 0)
                return true;

            string text = currentContent.ToString();
            string toResetText = text.Substring(text.Length - stepsToGoBack);
            string newCurrent = text.Substring(0, text.Length - stepsToGoBack);
            char firstChar = newCurrent.Length > 0 ? newCurrent[newCurrent.Length - 1] : ContentReader.EndOfInput;
            int startIndex = firstChar == ContentReader.EndOfInput ? currentStartIndex - 1 : currentStartIndex;

            // Update current 'token'.
            currentContent.Clear();
            currentContent.Append(newCurrent);

            // Prepare temporal reader.
            currentLength = currentContent.Length;
            currentReader.Push(new StringReader(firstChar + toResetText, startIndex));

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
