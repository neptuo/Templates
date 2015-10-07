using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Neptuo.Activators;
using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers.IO;
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
        private readonly TokenizerContext tokenizerContext;

        public TemplateClassifier(IClassificationTypeRegistryService registry, ITextBuffer buffer)
        {
            this.buffer = buffer;
            curlyBrace = registry.GetClassificationType(TemplateClassificationType.CurlyBrace);
            curlyContent = registry.GetClassificationType(TemplateClassificationType.CurlyContent);
            curlyError = registry.GetClassificationType(TemplateClassificationType.CurlyError);
            
            this.tokenizerContext = new TokenizerContext();
        }

        public event EventHandler<ClassificationChangedEventArgs> ClassificationChanged
        {
            add { }
            remove { }
        }

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
                IList<Token> tokens = tokenizerContext.Tokenize(buffer);
                foreach (Token token in tokens)
                {
                    if (!token.IsVirtual)
                    {
                        if (token.Type == CurlyTokenType.OpenBrace || token.Type == CurlyTokenType.CloseBrace)
                            addSpan(new ClassificationSpan(new SnapshotSpan(span.Snapshot, token.TextSpan.StartIndex, token.TextSpan.Length), curlyBrace));
                        else if (token.HasError)
                            addSpan(new ClassificationSpan(new SnapshotSpan(span.Snapshot, token.TextSpan.StartIndex, token.TextSpan.Length), curlyError));
                        else if (token.Type != CurlyTokenType.Literal)
                            addSpan(new ClassificationSpan(new SnapshotSpan(span.Snapshot, token.TextSpan.StartIndex, token.TextSpan.Length), curlyContent));
                    }
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
