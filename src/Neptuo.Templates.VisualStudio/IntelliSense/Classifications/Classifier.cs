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
    interface class Classifier : IClassifier
    {
        private readonly ITextBuffer buffer;
        private readonly IClassificationType curlyBrace, curlyContent, curlyError;
        private readonly TokenizerContext tokenizerContext;

        public Classifier(IClassificationTypeRegistryService registry, ITextBuffer buffer)
        {
            this.buffer = buffer;
            curlyBrace = registry.GetClassificationType(ClassificationType.CurlyBrace);
            curlyContent = registry.GetClassificationType(ClassificationType.CurlyContent);
            curlyError = registry.GetClassificationType(ClassificationType.CurlyError);
            
            this.tokenizerContext = new TokenizerContext();
        }

        public event EventHandler<ClassificationChangedEventArgs> ClassificationChanged
        {
            add { }
            remove { }
        }

        public IList<ClassificationSpan> GetClassificationSpans(SnapshotSpan span)
        {
            List<ClassificationSpan> result = new List<ClassificationSpan>();
            try
            {
                IList<Token> tokens = tokenizerContext.Tokenize(buffer);
                Token previousToken = null;
                foreach (Token token in tokens)
                {
                    IClassificationType type = MapTokenToClassificationType(token, previousToken);
                    if (type != null)
                        result.Add(new ClassificationSpan(new SnapshotSpan(span.Snapshot, token.TextSpan.StartIndex, token.TextSpan.Length), type));

                    previousToken = token;
                }
            }
            catch (Exception)
            { }

            return result;
        }

        private IClassificationType MapTokenToClassificationType(Token token, Token previousToken)
        {
            if (token.IsVirtual)
                return null;

            if (token.Type == CurlyTokenType.OpenBrace || token.Type == CurlyTokenType.CloseBrace)
                return curlyBrace;
            else if (token.HasError)
                return curlyError;
            else if (token.Type != CurlyTokenType.Literal || (previousToken != null && previousToken.Type == CurlyTokenType.AttributeValueSeparator))
                return curlyContent;

            return null;
        }
    }

    public enum CurlyTokenState
    {
        Other,
        Open,
        Name
    }
}
