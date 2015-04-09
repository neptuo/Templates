using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Tokenizers
{
    /// <summary>
    /// Parses content as single text token.
    /// </summary>
    public class PlainComposableTokenizer : IComposableTokenizer
    {
        public bool Accept(char input, ComposableTokenizerContext context)
        {
            return true;
        }

        public void Finalize(ComposableTokenizerContext context)
        {
            context.TryCreateToken(this, ComposableTokenType.Text);
        }
    }
}
