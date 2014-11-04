using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Describes position/offset in source global source content.
    /// </summary>
    public interface ISourceLineInfo
    {
        /// <summary>
        /// Line number.
        /// </summary>
        int LineNumber { get; }

        /// <summary>
        /// Index at line.
        /// </summary>
        int ColumnIndex { get; }
    }
}
