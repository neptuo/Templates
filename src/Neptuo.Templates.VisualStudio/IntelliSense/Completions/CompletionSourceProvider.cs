using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.VisualStudio.IntelliSense.Completions
{
    [Order(Before = "default")]
    [Export(typeof(ICompletionSourceProvider))]
    [Name(TemplateContentType.ContentType)]
    [ContentType(TemplateContentType.ContentType)]
    internal class CompletionSourceProvider : ICompletionSourceProvider
    {
        [Import]
        public IGlyphService GlyphService { get; set; }

        public ICompletionSource TryCreateCompletionSource(ITextBuffer textBuffer)
        {
            TokenContext tokenContext = textBuffer.Properties.GetOrCreateSingletonProperty(() => new TokenContext(textBuffer));
            return textBuffer.Properties.GetOrCreateSingletonProperty(() => new CompletionSource(tokenContext, textBuffer, GlyphService));
        }
    }
}
