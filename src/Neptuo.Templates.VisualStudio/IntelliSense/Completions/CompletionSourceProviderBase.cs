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
        public ICompletionSource TryCreateCompletionSource(ITextBuffer textBuffer)
        {
            TextContext textContext = textBuffer.Properties.GetOrCreateSingletonProperty(() => new TextContext(textBuffer));
            TokenContext tokenContext = textBuffer.Properties.GetOrCreateSingletonProperty(() => new TokenContext(textContext, CreateTokenizer()));
            return textBuffer.Properties.GetOrCreateSingletonProperty(() => new CompletionSource(
                tokenContext,
                CreateCompletionProvider(textBuffer), 
                CreateTokenTriggerProvider(textBuffer), 
                textBuffer
            ));
        }

        protected abstract ITokenizer CreateTokenizer();

        protected abstract ICompletionProvider CreateCompletionProvider(ITextBuffer textBuffer);

        protected abstract ITokenTriggerProvider CreateTokenTriggerProvider(ITextBuffer textBuffer);
    }
}
