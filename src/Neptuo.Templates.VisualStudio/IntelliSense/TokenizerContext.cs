using Microsoft.VisualStudio.Text;
using Neptuo.Templates.Compilation.Parsers.Tokenizers;
using Neptuo.Templates.Compilation.Parsers.Tokenizers.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.VisualStudio.IntelliSense
{
    public class TokenizerContext
    {
        private readonly ComposableTokenizer tokenizer;

        public TokenizerContext()
        {
            this.tokenizer = new ComposableTokenizer();
            tokenizer.Add(new CurlyTokenizer());
            tokenizer.Add(new PlainTokenizer());
        }

        public IList<ComposableToken> Tokenize(ITextBuffer textBuffer)
        {
            string textContent = textBuffer.CurrentSnapshot.GetText();
            return tokenizer.Tokenize(new StringReader(textContent), new FakeTokenizerContext());
        }
    }
}
