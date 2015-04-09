using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Tokenizers
{
    public interface IComposableTokenizer
    {
        bool Accept(char input, ComposableTokenizerContext context);

        void Finalize(ComposableTokenizerContext context);
    }
}
