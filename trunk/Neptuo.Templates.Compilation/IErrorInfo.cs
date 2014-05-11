using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation
{
    /// <summary>
    /// Describes error info.
    /// </summary>
    public interface IErrorInfo
    {
        /// <summary>
        /// Line where error occured.
        /// </summary>
        int Line { get; }

        /// <summary>
        /// Column where error occured.
        /// </summary>
        int Column { get; }

        /// <summary>
        /// Error index.
        /// </summary>
        string ErrorNumber { get; }

        /// <summary>
        /// Error message.
        /// </summary>
        string ErrorText { get; }
    }
}
