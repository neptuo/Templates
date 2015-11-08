using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
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
    public abstract class ViewControllerProviderBase : IVsTextViewCreationListener
    {
        [Import]
        internal IVsEditorAdaptersFactoryService AdapterService { get; set; }
        [Import]
        internal ICompletionBroker CompletionBroker { get; set; }
        [Import]
        internal SVsServiceProvider ServiceProvider { get; set; }

        public void VsTextViewCreated(IVsTextView textViewAdapter)
        {
            ITextView textView = AdapterService.GetWpfTextView(textViewAdapter);
            if (textView == null)
                return;

            TokenContext tokenContext = textView.TextBuffer.Properties.GetOrCreateSingletonProperty(() => new TokenContext(textView.TextBuffer, CreateTokenizer()));
            textView.Properties.GetOrCreateSingletonProperty(() => new ViewController(
                tokenContext, 
                CreateTokenTriggerProvider(textView.TextBuffer), 
                textViewAdapter, 
                textView, 
                CompletionBroker, 
                ServiceProvider
            ));
        }

        protected abstract ITokenizer CreateTokenizer();

        protected abstract ITokenTriggerProvider CreateTokenTriggerProvider(ITextBuffer textBuffer);
    }
}
