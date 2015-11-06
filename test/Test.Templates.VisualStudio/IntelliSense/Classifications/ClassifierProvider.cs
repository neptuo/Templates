using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.VisualStudio.IntelliSense.Classifications
{
    [Export(typeof(IClassifierProvider))]
    [ContentType(ContentType.TextValue)]
    internal class ClassifierProvider : IClassifierProvider
    {
        [Import]
        internal IClassificationTypeRegistryService Registry { get; set; }

        public IClassifier GetClassifier(ITextBuffer textBuffer)
        {
            TokenContext tokenContext = textBuffer.Properties.GetOrCreateSingletonProperty(() => new TokenContext(textBuffer));
            return textBuffer.Properties.GetOrCreateSingletonProperty(() => new Classifier(tokenContext, Registry, textBuffer));
        }
    }
}
