using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation
{
    /// <summary>
    /// Extesion of <see cref="ErrorInfo"/> with an exception which caused the error.
    /// </summary>
    public class ExceptionErrorInfo : ErrorInfo
    {
        /// <summary>
        /// The exception which caused the error.
        /// </summary>
        public Exception Exception { get; private set; }

        /// <summary>
        /// Creates new instance with <paramref name="exception"/>.
        /// </summary>
        /// <param name="exception">The exception which caused the error.</param>
        public ExceptionErrorInfo(Exception exception)
            : base(exception.Message)
        {
            Guard.NotNull(exception, "exception");
            Exception = exception; 
        }

        /// <summary>
        /// Creates new instance with <paramref name="exception"/>.
        /// </summary>
        /// <param name="line">Line number of error.</param>
        /// <param name="column">Column index of error.</param>
        /// <param name="exception">The exception which caused the error.</param>
        public ExceptionErrorInfo(int line, int column, Exception exception)
            : base(line, column, exception.Message)
        {
            Guard.NotNull(exception, "exception");
            Exception = exception;
        }
    }
}
