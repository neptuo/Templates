using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation
{
    /// <summary>
    /// Exception that contains information about position in template.
    /// </summary>
    [Serializable]
    public class SourceCodeException : Exception, ISourceCodeException
    {
        /// <summary>
        /// Line number in template source where error occured.
        /// </summary>
        public int LineNumber { get; set; }

        /// <summary>
        /// Line column index in template source where error occured.
        /// </summary>
        public int LinePosition { get; set; }

        /// <summary>
        /// Create new instance with error described as <paramref name="message"/> on line <paramref name="lineNumber"/> at column <paramref name="linePosition"/>.
        /// </summary>
        /// <param name="message">Text describtion of error.</param>
        /// <param name="lineNumber">Line number in template source where error occured.</param>
        /// <param name="linePosition">Line column index in template source where error occured.</param>
        public SourceCodeException(string message, int lineNumber, int linePosition)
            : base(message)
        {
            LineNumber = lineNumber;
            LinePosition = linePosition;
        }

        /// <summary>
        /// Create new instance with error described as <paramref name="message"/> on line <paramref name="lineNumber"/> at column <paramref name="linePosition"/>.
        /// </summary>
        /// <param name="message">Text describtion of error.</param>
        /// <param name="lineNumber">Line number in template source where error occured.</param>
        /// <param name="linePosition">Line column index in template source where error occured.</param>
        public SourceCodeException(string message, int lineNumber, int linePosition, Exception inner)
            : base(message, inner)
        {
            LineNumber = lineNumber;
            LinePosition = linePosition;
        }

        protected SourceCodeException(SerializationInfo info, StreamingContext context)
            : base(info, context) 
        { }
    }
}
