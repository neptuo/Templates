using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation
{
    public class CodeDomViewServiceException : Exception
    {
        public IEnumerable<IErrorInfo> Errors { get; private set; }
        public string ViewContent { get; private set; }

        public CodeDomViewServiceException(string message, IEnumerable<IErrorInfo> errors = null, string viewContent = null)
            : base(message)
        {
            Errors = errors ?? new List<IErrorInfo>
            {
                new ErrorInfo(message)
            };
            ViewContent = viewContent;
        }
    }
}
