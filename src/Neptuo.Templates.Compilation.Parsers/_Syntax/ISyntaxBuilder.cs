using Neptuo.Templates.Compilation.Parsers.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public interface ISyntaxBuilder
    {
        ISyntaxNode Build(IList<ComposableToken> tokens, int startIndex);
    }
}
