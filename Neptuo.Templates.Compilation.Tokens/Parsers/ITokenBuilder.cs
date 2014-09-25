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
        /// <returns><c>true</c> if succeed and token can be used; <c>false</c> otherwise.</returns>
        bool Parse(ITokenBuilderContext context, Token extension);
    }
}
