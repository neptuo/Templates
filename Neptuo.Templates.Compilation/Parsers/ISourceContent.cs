using Neptuo.ComponentModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Describes input into <see cref="IParserService"/>.
    /// </summary>
    public interface ISourceContent
    {
        /// <summary>
        /// Gets new instance of reader for reading source content.
        /// </summary>
        TextReader CreateContentReader();

        /// <summary>
        /// Gets information about offset in globally processing content.
        /// </summary>
        ISourceLineInfo GlobalSourceInfo { get; }
    }
}
