using Neptuo.Templates.Compilation.Parsers.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.SyntaxTrees
{
    public interface ISyntaxNode
    {
        IReadOnlyList<ComposableToken> Tokens { get; }
        IReadOnlyList<ComposableToken> LeadingTrivia { get; }
        IReadOnlyList<ComposableToken> TrailingTrivia { get; }
    }
}
