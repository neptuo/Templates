using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation
{
    public class ExceptionErrorInfo : ErrorInfo
    {
        public Exception Exception { get; set; }

        public ExceptionErrorInfo(Exception exception)
            : base(exception.Message)
        {
            Exception = exception; 
        }

        public ExceptionErrorInfo(int line, int column, Exception exception)
            : base(line, column, exception.Message)
        {
            Exception = exception;
        }
    }
}
