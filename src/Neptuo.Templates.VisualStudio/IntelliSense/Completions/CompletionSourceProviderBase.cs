using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.VisualStudio.IntelliSense.Completions
{
    public abstract class CompletionSourceProviderBase : ICompletionSourceProvider
    {
        [Import]
        public IGlyphService GlyphService { get; set; }

        public ICompletionSource TryCreateCompletionSource(ITextBuffer textBuffer)
        {
            CurlyProvider curlyProvider = new CurlyProvider(GlyphService);

            TokenContext tokenContext = textBuffer.Properties.GetOrCreateSingletonProperty(() => new TokenContext(textBuffer, CreateTokenizer()));
            return textBuffer.Properties.GetOrCreateSingletonProperty(() => new CompletionSource(tokenContext, curlyProvider, curlyProvider, textBuffer));
        }

        protected abstract ITokenizer CreateTokenizer();
    }
}
