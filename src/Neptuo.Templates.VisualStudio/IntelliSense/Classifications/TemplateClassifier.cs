using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Neptuo.Templates.Compilation.Parsers.Tokenizers;
using Neptuo.Templates.Compilation.Parsers.Tokenizers.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.VisualStudio.IntelliSense.Classifications
{
    public class TemplateClassifier : IClassifier
    {
        private readonly ITextBuffer buffer;
        private readonly IClassificationType curlyBrace, curlyContent, curlyError;
        private readonly ComposableTokenizer tokenizer;

        public TemplateClassifier(IClassificationTypeRegistryService registry, ITextBuffer buffer)
        {
            this.tokenizer = new ComposableTokenizer();
            tokenizer.Add(new CurlyComposableTokenizer());
            this.buffer = buffer;
            curlyBrace = registry.GetClassificationType(TemplateClassificationType.CurlyBrace);
            curlyContent = registry.GetClassificationType(TemplateClassificationType.CurlyContent);
            curlyError = registry.GetClassificationType(TemplateClassificationType.CurlyError);
        }

        public event EventHandler<ClassificationChangedEventArgs> ClassificationChanged;

        public IList<ClassificationSpan> GetClassificationSpans(SnapshotSpan span)
        {
            var spans = new List<ClassificationSpan>();
            Action<ClassificationSpan> addSpan = s =>
            {
                if (s.Span.IntersectsWith(span))
                {
                    spans.Add(s);
                }
            };

            try
            {
                IList<ComposableToken> tokens = tokenizer.Tokenize(new StringContentReader(buffer.CurrentSnapshot.GetText()), new FakeTokenizerContext());
                foreach (ComposableToken token in tokens)
                {
                    if (token.Type == CurlyComposableTokenizer.TokenType.OpenBrace || token.Type == CurlyComposableTokenizer.TokenType.CloseBrace)
                        addSpan(new ClassificationSpan(new SnapshotSpan(span.Snapshot, token.ContentInfo.StartIndex, token.ContentInfo.Length), curlyBrace));
                    else if(token.Type == CurlyComposableTokenizer.TokenType.Error)
                        addSpan(new ClassificationSpan(new SnapshotSpan(span.Snapshot, token.ContentInfo.StartIndex, token.ContentInfo.Length), curlyError));
                    else if (token.Type != CurlyComposableTokenizer.TokenType.Text)
                        addSpan(new ClassificationSpan(new SnapshotSpan(span.Snapshot, token.ContentInfo.StartIndex, token.ContentInfo.Length), curlyContent));
                }
            }
            catch (Exception)
            { }

            return spans;
        }
    }

    public enum CurlyTokenState
    {
        Other,
        Open,
        Name
    }
}
