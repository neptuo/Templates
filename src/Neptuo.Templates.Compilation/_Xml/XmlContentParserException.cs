using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation
{
    /// <summary>
    /// Exception used in <see cref="Parsers.XmlContentParser"/>.
    /// </summary>
    [Serializable]
    public class XmlContentParserException : SourceCodeException
    {
        /// <summary>
        /// Create new instance with error described as <paramref name="message"/> on line <paramref name="lineNumber"/> at column <paramref name="linePosition"/>.
        /// </summary>
        /// <param name="message">Text describtion of error.</param>
        /// <param name="lineNumber">Line number in template source where error occured.</param>
        /// <param name="linePosition">Line column index in template source where error occured.</param>
        public XmlContentParserException(string message, int lineNumber, int linePosition)
            : base(message, lineNumber, linePosition)
        { }
    }
}
