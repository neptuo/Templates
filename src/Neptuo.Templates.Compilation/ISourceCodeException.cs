using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation
{
    /// <summary>
    /// Exception that contains information about position in template.
    /// </summary>
    public interface ISourceCodeException
    {
        /// <summary>
        /// Source code line number.
        /// </summary>
        int LineNumber { get; }

        /// <summary>
        /// Source code line position (index).
        /// </summary>
        int LineIndex { get; }

        /// <summary>
        /// Error message.
        /// </summary>
        string Message { get; }
    }
}
