using Neptuo.Templates.Compilation.Parsers.Syntax.Nodes;
using Neptuo.Text;
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
        /// <param name="currentNode">The syntax node, where completion is to be provided.</param>
        /// <param name="currentToken">The token, inside <paramref name="currenctNode"/>, where completion is to be provided.</param>
        /// <returns>Enumeration of completion items for cursor at <paramref name="currentNode"/>.</returns>
        IEnumerable<ICompletion> GetCompletions(INode currentNode, Token currentToken);
    }
}
