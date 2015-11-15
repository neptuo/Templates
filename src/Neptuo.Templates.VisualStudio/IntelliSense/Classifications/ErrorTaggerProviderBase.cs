using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;
using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.VisualStudio.IntelliSense.Classifications
{
    public abstract class ErrorTaggerProviderBase : ITaggerProvider
    {
        public ITagger<T> CreateTagger<T>(ITextBuffer textBuffer)
            where T : ITag
        {
            TextContext textContext = textBuffer.Properties.GetOrCreateSingletonProperty(() => new TextContext(textBuffer));
            TokenContext tokenContext = textBuffer.Properties.GetOrCreateSingletonProperty(() => new TokenContext(textContext, CreateTokenizer()));
            return (ITagger<T>)textBuffer.Properties.GetOrCreateSingletonProperty(() => new ErrorTagger(tokenContext, textBuffer));
        }

        protected abstract ITokenizer CreateTokenizer();
    }
}
