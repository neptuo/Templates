using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation
{
    [Serializable]
    public class SourceCodeException : Exception, ISourceCodeException
    {
        public int LineNumber { get; set; }
        public int LinePosition { get; set; }

        public SourceCodeException() 
        { }

        public SourceCodeException(string message)
            : base(message)
        { }

        public SourceCodeException(string message, int lineNumber, int linePosition)
            : base(message)
        {
            LineNumber = lineNumber;
            LinePosition = linePosition;
        }

        public SourceCodeException(string message, Exception inner) 
            : base(message, inner) 
        { }

        protected SourceCodeException(SerializationInfo info, StreamingContext context)
            : base(info, context) 
        { }
    }
}
