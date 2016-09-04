using Neptuo.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.VisualStudio.IntelliSense.Completions
{
    /// <summary>
    /// Provider for automatic text completion.
    /// </summary>
    public interface IAutomaticCompletionProvider
    {
        /// <summary>
        /// Tries to provide completion based on <paramref name="currentToken"/>.
        /// </summary>
        /// <param name="currentToken">Current token cursor.</param>
        /// <param name="cursorPosition">Current cursor relation position to the start of <paramref name="currentToken"/>.</param>
        /// <param name="completion">Automatic completion to be applied.</param>
        /// <returns><c>true</c> if automatic completion should be executed; <c>false</c> otherwise.</returns>
        bool TryGet(TokenCursor currentToken, RelativePosition cursorPosition, out IAutomaticCompletion completion);
    }
}
