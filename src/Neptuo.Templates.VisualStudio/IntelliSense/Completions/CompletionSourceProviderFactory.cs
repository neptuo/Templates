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
    [Export(typeof(CompletionSourceProviderFactory))]
    public class CompletionSourceProviderFactory
    {
        public bool TryGet(ITextBuffer textBuffer, out ICompletionSource completionSource)
        {
            Ensure.NotNull(textBuffer, "textBuffer");
            return textBuffer.Properties.TryGetProperty(typeof(CompletionSource), out completionSource);
        }

        public ICompletionSource Create(ITextBuffer textBuffer, ITokenizer tokenizer, ICompletionProvider completionProvider, ICompletionTriggerProvider completionTriggerProvider)
        {
            return textBuffer.Properties.GetOrCreateSingletonProperty(() =>
            {
                TextContext textContext = textBuffer.Properties
                    .GetOrCreateSingletonProperty(() => new TextContext(textBuffer));

                TokenContext tokenContext = textBuffer.Properties
                    .GetOrCreateSingletonProperty(() => new TokenContext(textContext, tokenizer));

                return new CompletionSource(
                    tokenContext,
                    completionProvider,
                    completionTriggerProvider,
                    textBuffer
                );
            });
        }
    }
}
