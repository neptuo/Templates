using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation
{
    /// <summary>
    /// Exception used in compilation process in <see cref="CodeDomViewService"/>.
    /// </summary>
    public class CodeDomViewServiceException : Exception
    {
        /// <summary>
        /// List of errors that occured during compilation.
        /// </summary>
        public IEnumerable<IErrorInfo> Errors { get; private set; }

        /// <summary>
        /// Template content if available.
        /// </summary>
        public string ViewContent { get; private set; }

        /// <summary>
        /// Generated source code if available.
        /// </summary>
        public string SourceCode { get; private set; }

        /// <summary>
        /// Creates new instance.
        /// </summary>
        /// <param name="message">Custom text message.</param>
        /// <param name="errors">List of errors that occured during compilation.</param>
        /// <param name="viewContent">Template content if available.</param>
        /// <param name="sourceCode">Generated source code if available.</param>
        public CodeDomViewServiceException(string message, IEnumerable<IErrorInfo> errors = null, string viewContent = null, string sourceCode = null)
            : base(message)
        {
            Errors = errors ?? new List<IErrorInfo>
            {
                new ErrorInfo(message)
            };
            ViewContent = viewContent;
            SourceCode = sourceCode;
        }
    }
}
