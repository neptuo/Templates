using Neptuo.Text.Positions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation
{
    /// <summary>
    /// Describes input into <see cref="IViewService"/>.
    /// </summary>
    public interface ISourceContent
    {
        /// <summary>
        /// Gets new instance of reader for reading source content.
        /// </summary>
        string TextContent { get; }

        /// <summary>
        /// Gets information about offset in globally processing content.
        /// </summary>
        IDocumentPoint GlobalSourceInfo { get; }
    }
}
