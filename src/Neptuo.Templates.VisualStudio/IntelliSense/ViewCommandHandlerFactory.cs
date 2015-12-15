using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Neptuo.Activators;
using Neptuo.Templates.Compilation.Parsers.Syntax.Nodes;
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

        private bool TryGetTextView(IVsTextView textViewAdapter, out ITextView textView)
        {
            textView = AdapterService.GetWpfTextView(textViewAdapter);
            return textView != null;
        }

        public bool TryGetTextBuffer(IVsTextView textViewAdapter, out ITextBuffer textBuffer)
        {
            ITextView textView;
            if (TryGetTextView(textViewAdapter, out textView))
            {
                textBuffer = textView.TextBuffer;
                return true;
            }

            textBuffer = null;
            return false;
        }

        public bool TryGet(IVsTextView textViewAdapter, out IViewCommandHandler viewCommandHandler)
        {
            ITextView textView;
            if (TryGetTextView(textViewAdapter, out textView))
                return textView.Properties.TryGetProperty(typeof(ViewCommandHandler), out viewCommandHandler);

            viewCommandHandler = null;
            return false;
        }

        public IViewCommandHandler Create(IVsTextView textViewAdapter, ITokenizer tokenizer, ICompletionTriggerProvider tokenTriggerProvider, IAutomaticCompletionProvider automaticCompletionProvider)
        {
            ITextView textView;
            if (TryGetTextView(textViewAdapter, out textView))
            {
                return textView.Properties.GetOrCreateSingletonProperty(() =>
                {
                    ITextBuffer textBuffer = textView.TextBuffer;

                    TextContext textContext = textBuffer.Properties
                        .GetOrCreateSingletonProperty(() => new TextContext(textBuffer));

                    TokenContext tokenContext = textBuffer.Properties
                        .GetOrCreateSingletonProperty(() => new TokenContext(textContext, tokenizer));

                    return new ViewCommandHandler(
                        new CompletionContext(
                            tokenContext,
                            tokenTriggerProvider,
                            automaticCompletionProvider,
                            textView,
                            CompletionBroker
                        ),
                        textViewAdapter,
                        textView,
                        ServiceProvider
                    );
                });
            }

            throw Ensure.Exception.NotSupported("Unnable to attach view command handler without ITextView.");
        }
    }
}
