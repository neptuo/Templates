using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.VisualStudio.IntelliSense.Completions
{
    public interface ICompletionProvider
    {

        /// <summary>
        /// Returns completion items for cursor at <paramref name="currentToken"/> in <paramref name="tokens"/>.
        /// </summary>
        /// <param name="tokens">List of current tokens.</param>
        /// <param name="currentToken">Token, in which cursor is.</param>
        /// <returns>Enumeration of completion items for cursor at <paramref name="currentToken"/> in <paramref name="tokens"/>.</returns>
        IEnumerable<ICompletion> GetCompletions(IReadOnlyList<Token> tokens, Token currentToken);
    }
}
