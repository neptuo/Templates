using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Neptuo.Activators;
using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using Neptuo.Templates.VisualStudio.IntelliSense.Completions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.VisualStudio.IntelliSense
{
    [Export(typeof(ViewCommandHandlerFactory))]
    public class ViewCommandHandlerFactory
    {
        [Import]
        internal IVsEditorAdaptersFactoryService AdapterService { get; set; }
        [Import]
        internal ICompletionBroker CompletionBroker { get; set; }
        [Import]
        internal SVsServiceProvider ServiceProvider { get; set; }

        public void Attach(IVsTextView textViewAdapter, ITokenizerFactory tokenizerFactory, ICompletionTriggerProviderFactory tokenTriggerProviderFactory, IAutomaticCompletionProviderFactory automaticCompletionProviderFactory)
        {
            ITextView textView = AdapterService.GetWpfTextView(textViewAdapter);
            if (textView == null)
                return;

            ITextBuffer textBuffer = textView.TextBuffer;

            TextContext textContext = textBuffer.Properties.GetOrCreateSingletonProperty(() => new TextContext(textBuffer));
            TokenContext tokenContext = textBuffer.Properties.GetOrCreateSingletonProperty(() => new TokenContext(textContext, tokenizerFactory.Create(textBuffer)));

            ViewCommandHandler viewController = textView.Properties.GetOrCreateSingletonProperty(() => new ViewCommandHandler(
                new CompletionContext(
                    tokenContext,
                    tokenTriggerProviderFactory.Create(textBuffer),
                    automaticCompletionProviderFactory.Create(textBuffer),
                    textView,
                    CompletionBroker
                ),
                textViewAdapter,
                textView,
                ServiceProvider
            ));
        }
    }
}
