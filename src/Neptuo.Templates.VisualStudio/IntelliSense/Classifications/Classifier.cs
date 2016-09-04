using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Neptuo.Activators;
using Neptuo.Text;
using Neptuo.Text.IO;
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
        private readonly IClassificationType curlyBrace, curlyName, curlyAttributeName;
        private readonly TokenContext tokenContext;

        private readonly ITokenClassificationProvider tokenClassification;
        private readonly IClassificationTypeRegistryService registry;
        private readonly Dictionary<TokenType, IClassificationType> typeCache = new Dictionary<TokenType, IClassificationType>();

        public Classifier(TokenContext tokenContext, IClassificationTypeRegistryService registry, ITextBuffer buffer, ITokenClassificationProvider tokenClassification)
        {
            this.tokenContext = tokenContext;
            this.registry = registry;
            this.buffer = buffer;
            this.tokenClassification = tokenClassification;
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

            IClassificationType result;
            if (typeCache.TryGetValue(token.Type, out result))
                return result;

            string classificationName;
            if (tokenClassification.TryGet(token.Type, out classificationName))
            {
                result = registry.GetClassificationType(classificationName);
                typeCache[token.Type] = result;
                return result;
            }

            return null;
        }
    }
}
