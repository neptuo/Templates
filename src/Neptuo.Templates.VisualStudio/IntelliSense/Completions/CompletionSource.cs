using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Operations;
using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers.IO;
using Neptuo.Templates.Compilation.Parsers.Syntax.Nodes;
using Neptuo.Templates.Compilation.Parsers.Syntax.Nodes.Visitors;

namespace Neptuo.Templates.VisualStudio.IntelliSense.Completions
{
    internal class CompletionSource : DisposableBase, ICompletionSource
    {
        public const string Moniker = "ntemplate";

        private readonly SyntaxContext syntaxContext;
        private readonly ICompletionProvider completionProvider;
        private readonly ISyntaxNodeVisitor nodeVisitor;
        private readonly List<ITokenTrigger> triggers;
        private readonly ITextBuffer textBuffer;

        public CompletionSource(SyntaxContext syntaxContext, ICompletionProvider completionProvider, ISyntaxNodeVisitor nodeVisitor, ICompletionTriggerProvider triggerProvider, ITextBuffer textBuffer)
        {
            this.textBuffer = textBuffer;
            this.completionProvider = completionProvider;
            this.nodeVisitor = nodeVisitor;
            this.triggers = triggerProvider.GetStartTriggers().ToList();
            this.syntaxContext = syntaxContext;
        }

        public void AugmentCompletionSession(ICompletionSession session, IList<CompletionSet> completionSets)
        {
            List<Completion> result = new List<Completion>();
            ISyntaxNode rootNode = syntaxContext.RootNode;
            if (rootNode == null)
                return;

            SnapshotPoint cursorPosition = session.TextView.Caret.Position.BufferPosition;

            TokenAtPositionFinder finder = new TokenAtPositionFinder(cursorPosition);
            nodeVisitor.Visit(rootNode, finder);
            ISyntaxNode currentNode = finder.BestMatch;
            if (currentNode == null)
                return;

            Token currentToken = currentNode.GetTokens()
                .First(t => t.TextSpan.StartIndex <= cursorPosition && t.TextSpan.StartIndex + t.TextSpan.Length >= cursorPosition);

            IEnumerable<Completion> completions = completionProvider
                .GetCompletions(currentNode, currentToken)
                .Select(c => new Completion(c.DisplayText, c.InsertionText, c.DescriptionText, c.IconSource, String.Empty));

            result.AddRange(completions);

            CompletionSet newCompletionSet = new CompletionSet(
                Moniker,
                "Neptuo Templates",
                FindTokenSpanAtPosition(session.GetTriggerPoint(textBuffer), session, currentToken),
                result,
                null
            );

            if (completionSets.Any())
                completionSets.RemoveAt(0);

            newCompletionSet.SelectBestMatch();
            completionSets.Add(newCompletionSet);
        }

        private ITrackingSpan FindTokenSpanAtPosition(ITrackingPoint point, ICompletionSession session, Token currentToken)
        {
            int startIndex = currentToken.TextSpan.StartIndex;
            int length = Math.Max(0, currentToken.TextSpan.StartIndex + currentToken.TextSpan.Length - startIndex);

            ITokenTrigger trigger = triggers.FirstOrDefault(t => t.Type == currentToken.Type);
            if (trigger == null || !trigger.IsValueReplaced)
            {
                startIndex = point.GetPosition(textBuffer.CurrentSnapshot);
                length = 0;
            }

            return textBuffer.CurrentSnapshot.CreateTrackingSpan(
                startIndex,
                length,
                SpanTrackingMode.EdgeInclusive
            );
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
    }
}
