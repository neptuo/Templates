using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation
{
    public class CodeDomViewServiceException : Exception
    {
        public IEnumerable<IErrorInfo> Errors { get; private set; }

        public CodeDomViewServiceException(string message, IEnumerable<IErrorInfo> errors = null)
            : base(message)
        {
            Errors = errors;
        }
    }
}
