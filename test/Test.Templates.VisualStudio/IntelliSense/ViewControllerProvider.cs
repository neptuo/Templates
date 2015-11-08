using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Utilities;
using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using Neptuo.Templates.VisualStudio.IntelliSense;
using Neptuo.Templates.VisualStudio.IntelliSense.Completions;
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
    internal class ViewControllerProvider : ViewControllerProviderBase, IVsTextViewCreationListener
    {
        [Import]
        public IGlyphService GlyphService { get; set; }

        protected override ITokenizer CreateTokenizer()
        {
            return new TokenizerFactory().Create();
        }

        protected override ITokenTriggerProvider CreateTokenTriggerProvider(ITextBuffer textBuffer)
        {
            return textBuffer.Properties.GetOrCreateSingletonProperty(() => new CurlyProvider(GlyphService));
        }
    }
}
