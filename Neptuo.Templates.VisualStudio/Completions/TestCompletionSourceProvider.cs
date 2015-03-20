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

namespace Neptuo.Templates.VisualStudio.Completions
{
    [Export(typeof(ICompletionSourceProvider))]
    [Name("token completion")]
    [ContentType(TestContentTypeDefinition.ContentType)]
    internal class TestCompletionSourceProvider : ICompletionSourceProvider
    {
        [Import]
        internal ITextStructureNavigatorSelectorService NavigatorService { get; set; }
        
        public ICompletionSource TryCreateCompletionSource(ITextBuffer textBuffer)
        {
            return new TestCompletionSource(this, textBuffer);
        }
    }
}
