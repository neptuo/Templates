using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.VisualStudio.IntelliSense.Classifications
{
    [Export(typeof(ClassifierFactory))]
    public class ClassifierFactory
    {
        [Import]
        internal IClassificationTypeRegistryService Registry { get; set; }

        public bool TryGet(ITextBuffer textBuffer, out IClassifier classifier)
        {
            Ensure.NotNull(textBuffer, "textBuffer");
            return textBuffer.Properties.TryGetProperty(typeof(Classifier), out classifier);
        }

        public IClassifier Create(ITextBuffer textBuffer, ITokenizer tokenizer, ITokenClassificationProvider tokenClassificationProvider)
        {
            return textBuffer.Properties.GetOrCreateSingletonProperty(() =>
            {
                TextContext textContext = textBuffer.Properties
                    .GetOrCreateSingletonProperty(() => new TextContext(textBuffer));

                TokenContext tokenContext = textBuffer.Properties
                    .GetOrCreateSingletonProperty(() => new TokenContext(textContext, tokenizer));

                return new Classifier(
                    tokenContext,
                    Registry,
                    textBuffer,
                    tokenClassificationProvider
                );
            });
        }
    }
}
