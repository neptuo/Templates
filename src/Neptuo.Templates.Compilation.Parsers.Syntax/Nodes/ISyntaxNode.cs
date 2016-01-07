using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Nodes
{
    /// <summary>
    /// The item of the syntax tree.
    /// </summary>
    public interface ISyntaxNode
    {
        /// <summary>
        /// The parent syntax node.
        /// </summary>
        ISyntaxNode Parent { get; set; }

        /// <summary>
        /// Enumerates the all contained tokens.
        /// </summary>
        /// <returns>The enumeration of the all contained tokens.</returns>
        IEnumerable<Token> GetTokens();

        /// <summary>
        /// Gets list of the line leading trivia.
        /// </summary>
        IList<Token> LeadingTrivia { get; }

        /// <summary>
        /// Gets list of the line trailing trivia.
        /// </summary>
        IList<Token> TrailingTrivia { get; }
    }
}
