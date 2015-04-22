using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Tokenizers
{
    /// <summary>
    /// Provider for token types.
    /// </summary>
    public interface IComposableTokenTypeProvider
    {
        /// <summary>
        /// Returns enumeration of supported token types.
        /// </summary>
        /// <returns>Enumeration of supported token types.</returns>
        IEnumerable<ComposableTokenType> GetSupportedTokenTypes();
    }
}
