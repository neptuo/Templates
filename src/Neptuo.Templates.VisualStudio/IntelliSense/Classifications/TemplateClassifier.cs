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
        private readonly CurlyTokenizer tokenizer;

        public TemplateClassifier(IClassificationTypeRegistryService registry, ITextBuffer buffer)
        {
            this.tokenizer = new CurlyTokenizer();
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
                IList<CurlyToken> tokens = tokenizer.Tokenize(new StringContentReader(buffer.CurrentSnapshot.GetText()), new FakeTokenizerContext());

                CurlyTokenState state = CurlyTokenState.Other;
                foreach (CurlyToken token in tokens)
                {
                    // After error, continue from current state, or start orver?
                    if (state == CurlyTokenState.Other)
                    {
                        if (token.Type == CurlyTokenType.OpenBrace)
                        {
                            addSpan(new ClassificationSpan(new SnapshotSpan(span.Snapshot, token.ContentInfo.StartIndex, token.ContentInfo.Length), curlyBrace));
                            state = CurlyTokenState.Open;
                        }
                        else if (token.Type == CurlyTokenType.Error)
                        {
                            addSpan(new ClassificationSpan(new SnapshotSpan(span.Snapshot, token.ContentInfo.StartIndex, token.ContentInfo.Length), curlyError));
                        }
                    }
                    else if (state == CurlyTokenState.Open)
                    {
                        if (token.Type == CurlyTokenType.Name)
                        {
                            addSpan(new ClassificationSpan(new SnapshotSpan(span.Snapshot, token.ContentInfo.StartIndex, token.ContentInfo.Length), curlyContent));
                            state = CurlyTokenState.Name;
                        } 
                        else if (token.Type == CurlyTokenType.CloseBrace)
                        {
                            addSpan(new ClassificationSpan(new SnapshotSpan(span.Snapshot, token.ContentInfo.StartIndex, token.ContentInfo.Length), curlyBrace));
                            state = CurlyTokenState.Other;
                        }
                        else if (token.Type == CurlyTokenType.Error)
                        {
                            addSpan(new ClassificationSpan(new SnapshotSpan(span.Snapshot, token.ContentInfo.StartIndex, token.ContentInfo.Length), curlyError));
                        }
                    }
                    else if (state == CurlyTokenState.Name)
                    {
                        if (token.Type == CurlyTokenType.CloseBrace)
                        {
                            addSpan(new ClassificationSpan(new SnapshotSpan(span.Snapshot, token.ContentInfo.StartIndex, token.ContentInfo.Length), curlyBrace));
                            state = CurlyTokenState.Other;
                        }
                        else if(token.Type == CurlyTokenType.Error)
                        {
                            addSpan(new ClassificationSpan(new SnapshotSpan(span.Snapshot, token.ContentInfo.StartIndex, token.ContentInfo.Length), curlyError));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }

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
