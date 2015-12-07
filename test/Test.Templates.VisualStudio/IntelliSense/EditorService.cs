using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Utilities;
using Neptuo.Templates.VisualStudio.IntelliSense;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Templates.VisualStudio.IntelliSense
{
    [Export(typeof(IVsTextViewCreationListener))]
    [Name(ContentType.TextValue)]
    [ContentType(ContentType.TextValue)]
    [TextViewRole(PredefinedTextViewRoles.Interactive)]
    public class EditorService : IVsTextViewCreationListener
    {
        [Import]
        internal ViewCommandHandlerFactory ViewCommandHandlerFactory { get; set; }

        [Import]
        internal IGlyphService GlyphService { get; set; }

        private TokenizerFactory tokenizerFactory;
        private CompletionTriggerProviderFactory completionTriggerProviderFactory;

        private void Initialize()
        {
            tokenizerFactory = new TokenizerFactory();
            completionTriggerProviderFactory = new CompletionTriggerProviderFactory(GlyphService);
        }

        public void VsTextViewCreated(IVsTextView textViewAdapter)
        {
            Initialize();
            ViewCommandHandlerFactory.Attach(textViewAdapter, tokenizerFactory, completionTriggerProviderFactory, completionTriggerProviderFactory);
        }
    }
}
