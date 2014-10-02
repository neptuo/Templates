using Neptuo.Templates.Compilation.CodeObjects;
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
    public interface ITokenBuilder
    {
        /// <summary>
        /// Parses markup extension and creates AST.
        /// </summary>
        /// <param name="context">Context information.</param>
        /// <param name="extension">Token describing markup extension.</param>
        /// <returns>Parsed code object; <c>null</c> otherwise.</returns>
        ICodeObject Parse(ITokenBuilderContext context, Token extension);
    }
}
