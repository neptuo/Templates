using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Operations;
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

namespace Test.Templates.VisualStudio.IntelliSense.Completions
{
    [Export(typeof(ICompletionSourceProvider))]
    [Name(ContentType.TextValue)]
    [ContentType(ContentType.TextValue)]
    internal class CompletionSourceProvider : CompletionSourceProviderBase
    {
        [Import]
        public IGlyphService GlyphService { get; set; }

        protected override ITokenizer CreateTokenizer()
        {
            return new TokenizerFactory().Create();
        }

        protected override ICompletionProvider CreateCompletionProvider(ITextBuffer textBuffer)
        {
            return new CompletionProviderCollection()
                .Add(new AngleProvider(GlyphService))
                .Add(new CurlyProvider(GlyphService));
        }

        protected override ITokenTriggerProvider CreateTokenTriggerProvider(ITextBuffer textBuffer)
        {
            return new TokenTriggerProviderCollection()
                .AddStart(new AngleProvider(GlyphService))
                .AddStart(new CurlyProvider(GlyphService));
        }
    }
}
