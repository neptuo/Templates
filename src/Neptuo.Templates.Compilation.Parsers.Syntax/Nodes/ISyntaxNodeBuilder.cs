using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Nodes
{
    public interface ISyntaxNodeBuilder
    {
        ISyntaxNode Build(IList<Token> tokens, int startIndex, ISyntaxNodeBuilderContext context);
    }
}
