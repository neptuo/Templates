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
    public abstract class ClassifierProviderBase : IClassifierProvider
    {
        [Import]
        internal IClassificationTypeRegistryService Registry { get; set; }

        public IClassifier GetClassifier(ITextBuffer textBuffer)
        {
            TextContext textContext = textBuffer.Properties.GetOrCreateSingletonProperty(() => new TextContext(textBuffer));
            TokenContext tokenContext = textBuffer.Properties.GetOrCreateSingletonProperty(() => new TokenContext(textContext, CreateTokenizer()));
            return textBuffer.Properties.GetOrCreateSingletonProperty(() => new Classifier(
                tokenContext, 
                Registry, 
                textBuffer,
                CreateTokenClassificationProvider()
            ));
        }

        protected abstract ITokenClassificationProvider CreateTokenClassificationProvider();

        protected abstract ITokenizer CreateTokenizer();
    }
}
