using Neptuo.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Builder for markup extension.
    /// </summary>
    public interface IMarkupExtensionBuilder
    {
        /// <summary>
        /// Parses markup extension and creates AST.
        /// </summary>
        /// <param name="context">Context information.</param>
        /// <param name="extension">Token describing markup extension.</param>
        /// <returns>True if succeed.</returns>
        bool Parse(IMarkupExtensionBuilderContext context, Token extension);
    }
}
