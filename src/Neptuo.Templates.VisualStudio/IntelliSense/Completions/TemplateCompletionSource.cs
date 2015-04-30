using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Operations;
using Neptuo.ComponentModel;
using Neptuo.Templates.Compilation.Parsers.Tokenizers;
using Neptuo.Templates.Compilation.Parsers.Tokenizers.IO;

namespace Neptuo.Templates.VisualStudio.IntelliSense.Completions
{
    internal class TemplateCompletionSource : DisposableBase, ICompletionSource
    {
        public const string Moniker = "ntemplate";

        private readonly ITextBuffer textBuffer;
        private readonly TokenizerContext tokenizer;

        private readonly List<string> tokenNames = new List<string>() { "Binding", "StaticResource" };
        private readonly List<string> attributeNames = new List<string>() { "Path", "Converter", "Key" };

        public TemplateCompletionSource(ITextBuffer textBuffer)
        {
            this.textBuffer = textBuffer;
            this.tokenizer = new TokenizerContext();
        }

        public void AugmentCompletionSession(ICompletionSession session, IList<CompletionSet> completionSets)
        {
            List<Completion> result = new List<Completion>();
            IList<ComposableToken> tokens = tokenizer.Tokenize(textBuffer);

            SnapshotPoint cursorPosition = session.TextView.Caret.Position.BufferPosition;
            ComposableToken currentToken = tokens.FirstOrDefault(t => t.ContentInfo.StartIndex <= cursorPosition && t.ContentInfo.StartIndex + t.ContentInfo.Length >= cursorPosition);
            if (currentToken != null)
            {
                if(currentToken.Type == CurlyTokenType.Name || currentToken.Type == CurlyTokenType.OpenBrace)
                {
                    result.AddRange(tokenNames
                        .Where(n => n.StartsWith(currentToken.Text))
                        .Select(n => new Completion(n, n, n, null, ""))
                    );
                }
                else if (currentToken.Type == CurlyTokenType.AttributeName || currentToken.Type == CurlyTokenType.DefaultAttributeValue)
                {
                    result.AddRange(attributeNames
                        .Where(n => n.StartsWith(currentToken.Text))
                        .Select(n => new Completion(n, n, n, null, ""))
                    );
                }
            }

            CompletionSet newCompletionSet = new CompletionSet(
                Moniker,
                "Neptuo Templates",
                FindTokenSpanAtPosition(session.GetTriggerPoint(textBuffer), session),
                result,
                null
            );
            //completionSets.Add(MergeCompletionSets(completionSets, newCompletionSet));

            if (completionSets.Any())
                completionSets.RemoveAt(0);

            completionSets.Add(newCompletionSet);
        }

        private CompletionSet MergeCompletionSets(IList<CompletionSet> completionSets, CompletionSet newCompletionSet)
        {
            CompletionSet htmlCompletionSet = completionSets.FirstOrDefault();
            if (htmlCompletionSet != null)
            {
                CompletionSet mergedCompletionSet = new CompletionSet(
                    newCompletionSet.Moniker,
                    newCompletionSet.DisplayName,
                    newCompletionSet.ApplicableTo,
                    newCompletionSet.Completions.Concat(htmlCompletionSet.Completions).OrderBy(n => n.DisplayText),
                    htmlCompletionSet.CompletionBuilders
                );

                completionSets.Remove(htmlCompletionSet);
                return mergedCompletionSet;
            }

            return newCompletionSet;
        }

        private ITrackingSpan FindTokenSpanAtPosition(ITrackingPoint point, ICompletionSession session)
        {
            SnapshotPoint currentPoint = session
                .GetTriggerPoint(textBuffer)
                .GetPoint(textBuffer.CurrentSnapshot);

            return currentPoint.Snapshot.CreateTrackingSpan(
                currentPoint.Position, 
                0, 
                SpanTrackingMode.EdgeInclusive
            );
        }
    }
}
