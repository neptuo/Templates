using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Neptuo;
using Neptuo.Activators;
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
    public class CompletionTriggerProviderFactory : ICompletionTriggerProviderFactory, IAutomaticCompletionProviderFactory
    {
        private readonly IGlyphService glyphService;

        public CompletionTriggerProviderFactory(IGlyphService glyphService)
        {
            Ensure.NotNull(glyphService, "glyphService");
            this.glyphService = glyphService;
        }

        ICompletionTriggerProvider IFactory<ICompletionTriggerProvider, ITextBuffer>.Create(ITextBuffer textBuffer)
        {
            return textBuffer.Properties.GetOrCreateSingletonProperty(
                () => new CompletionTriggerProviderCollection()
                    .Add(new AngleProvider(glyphService))
                    .Add(new CurlyProvider(glyphService))
            );
        }

        IAutomaticCompletionProvider IFactory<IAutomaticCompletionProvider, ITextBuffer>.Create(ITextBuffer textBuffer)
        {
            return textBuffer.Properties.GetOrCreateSingletonProperty(
                () => new AutomaticCompletionProviderCollection()
                    .Add(new AngleProvider(glyphService))
                    .Add(new CurlyProvider(glyphService))
            );
        }
    }
}
