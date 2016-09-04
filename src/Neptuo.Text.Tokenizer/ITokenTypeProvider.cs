using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Text
{
    /// <summary>
    /// Provider for token types.
    /// </summary>
    public interface ITokenTypeProvider
    {
        /// <summary>
        /// Returns enumeration of supported token types.
        /// </summary>
        /// <returns>Enumeration of supported token types.</returns>
        IEnumerable<TokenType> GetSupportedTokenTypes();
    }
}
