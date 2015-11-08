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
        public ITagger<T> CreateTagger<T>(ITextBuffer buffer)
            where T : ITag
        {
            TokenContext tokenContext = buffer.Properties.GetOrCreateSingletonProperty(() => new TokenContext(buffer, CreateTokenizer()));
            return (ITagger<T>)buffer.Properties.GetOrCreateSingletonProperty(() => new ErrorTagger(tokenContext, buffer));
        }

        protected abstract ITokenizer CreateTokenizer();
    }
}
