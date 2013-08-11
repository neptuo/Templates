using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation
{
    [Serializable]
    public class BaseSourceCodeException : Exception, ISourceCodeException
    {
        public int LineNumber { get; set; }
        public int LinePosition { get; set; }

        public BaseSourceCodeException() 
        { }

        public BaseSourceCodeException(string message)
            : base(message)
        { }

        public BaseSourceCodeException(string message, int lineNumber, int linePosition)
            : base(message)
        {
            LineNumber = lineNumber;
            LinePosition = linePosition;
        }

        public BaseSourceCodeException(string message, Exception inner) 
            : base(message, inner) 
        { }

        protected BaseSourceCodeException(SerializationInfo info, StreamingContext context)
            : base(info, context) 
        { }
    }
}
