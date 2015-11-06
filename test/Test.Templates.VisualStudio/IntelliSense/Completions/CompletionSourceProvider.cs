using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Utilities;
using Neptuo.Templates.VisualStudio.IntelliSense;
using Neptuo.Templates.VisualStudio.IntelliSense.Completions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Templates.VisualStudio.IntelliSense.Completions
{
    [Export(typeof(ICompletionSourceProvider))]
    [Name(ContentType.TextValue)]
    [ContentType(ContentType.TextValue)]
    internal class CompletionSourceProvider : ICompletionSourceProvider
    {
        [Import]
        public IGlyphService GlyphService { get; set; }

        public ICompletionSource TryCreateCompletionSource(ITextBuffer textBuffer)
        {
            CurlyProvider curlyProvider = new CurlyProvider(GlyphService);

            TokenContext tokenContext = textBuffer.Properties.GetOrCreateSingletonProperty(() => new TokenContext(textBuffer));
            return textBuffer.Properties.GetOrCreateSingletonProperty(() => new CompletionSource(tokenContext, curlyProvider, curlyProvider, textBuffer));
        }
    }
}
