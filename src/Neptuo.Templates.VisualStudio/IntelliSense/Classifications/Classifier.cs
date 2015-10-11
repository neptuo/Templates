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
    internal class Classifier : IClassifier
    {
        private readonly ITextBuffer buffer;
        private readonly IClassificationType curlyBrace, curlyName, curlyAttributeName, curlyError;
        private readonly TokenContext tokenContext;

        public Classifier(TokenContext tokenContext, IClassificationTypeRegistryService registry, ITextBuffer buffer)
        {
            this.buffer = buffer;
            curlyBrace = registry.GetClassificationType(ClassificationType.CurlyBrace);
            curlyName = registry.GetClassificationType(ClassificationType.CurlyName);
            curlyAttributeName = registry.GetClassificationType(ClassificationType.CurlyAttributeName);
            curlyError = registry.GetClassificationType(ClassificationType.CurlyError);

            this.tokenContext = tokenContext;
        }

        public event EventHandler<ClassificationChangedEventArgs> ClassificationChanged
        {
            add { }
            remove { }
        }

        public IList<ClassificationSpan> GetClassificationSpans(SnapshotSpan span)
        {
            List<ClassificationSpan> result = new List<ClassificationSpan>();

            IList<Token> tokens = tokenContext.Tokens.ToList();
            Token previousToken = null;
            foreach (Token token in tokens)
            {
                IClassificationType type = MapTokenToClassificationType(token, previousToken);
                if (type != null)
                    result.Add(new ClassificationSpan(new SnapshotSpan(span.Snapshot, token.TextSpan.StartIndex, token.TextSpan.Length), type));

                previousToken = token;
            }

            return result;
        }

        private IClassificationType MapTokenToClassificationType(Token token, Token previousToken)
        {
            if (token.IsVirtual)
                return null;

            if (token.Type == CurlyTokenType.OpenBrace || token.Type == CurlyTokenType.CloseBrace || token.Type == CurlyTokenType.AttributeSeparator || token.Type == CurlyTokenType.AttributeValueSeparator)
                return curlyBrace;
            else if (token.Type == CurlyTokenType.Name || token.Type == CurlyTokenType.NamePrefix || token.Type == CurlyTokenType.NameSeparator)
                return curlyName;
            else if (token.Type == CurlyTokenType.AttributeName)
                return curlyAttributeName;
            else if (token.HasError)
                return curlyError;

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
