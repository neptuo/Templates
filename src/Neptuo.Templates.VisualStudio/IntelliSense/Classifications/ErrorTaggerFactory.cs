using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;
using Neptuo.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.VisualStudio.IntelliSense.Classifications
{
    [Export(typeof(ErrorTaggerFactory))]
    public class ErrorTaggerFactory
    {
        public bool TryGet(ITextBuffer textBuffer, out ITagger<IErrorTag> tagger)
        {
            Ensure.NotNull(textBuffer, "textBuffer");
            return textBuffer.Properties.TryGetProperty(typeof(ErrorTagger), out tagger);
        }

        public ITagger<IErrorTag> Create(ITextBuffer textBuffer, ITokenizer tokenizer)
        {
            return textBuffer.Properties.GetOrCreateSingletonProperty(() =>
            {
                TextContext textContext = textBuffer.Properties
                    .GetOrCreateSingletonProperty(() => new TextContext(textBuffer));

                TokenContext tokenContext = textBuffer.Properties
                    .GetOrCreateSingletonProperty(() => new TokenContext(textContext, tokenizer));

                return new ErrorTagger(tokenContext, textBuffer);
            });
        }
    }
}
