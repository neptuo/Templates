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

namespace Neptuo.Templates.VisualStudio.IntelliSense.Completions
{
    internal class CompletionSource : DisposableBase, ICompletionSource
    {
        public const string Moniker = "ntemplate";

        private readonly ITextBuffer textBuffer;
        private readonly IGlyphService glyphService;
        private readonly TokenContext tokenContext;

        private readonly List<string> tokenNames = new List<string>() { "Binding", "TemplateBinding", "Template", "Source", "StaticResource" };
        private readonly List<string> attributeNames = new List<string>() { "Path", "Converter", "Key" };

        public CompletionSource(TokenContext tokenContext, ITextBuffer textBuffer, IGlyphService glyphService)
        {
            this.textBuffer = textBuffer;
            this.glyphService = glyphService;
            this.tokenContext = tokenContext;
        }

        public void AugmentCompletionSession(ICompletionSession session, IList<CompletionSet> completionSets)
        {
            List<Completion> result = new List<Completion>();
            IList<Token> tokens = tokenContext.Tokens;

            SnapshotPoint cursorPosition = session.TextView.Caret.Position.BufferPosition;
            Token currentToken = tokens.FirstOrDefault(t => t.TextSpan.StartIndex <= cursorPosition && t.TextSpan.StartIndex + t.TextSpan.Length >= cursorPosition);
            if (currentToken != null)
            {
                if(currentToken.Type == CurlyTokenType.Name || currentToken.Type == CurlyTokenType.OpenBrace)
                {
                    result.AddRange(tokenNames
                        .Where(n => currentToken.Type == CurlyTokenType.OpenBrace || n.StartsWith(currentToken.Text))
                        .Select(n => CreateItem(currentToken, n, StandardGlyphGroup.GlyphGroupClass))
                    );
                }
                else if (currentToken.Type == TokenType.Whitespace || currentToken.Type == CurlyTokenType.AttributeName || currentToken.Type == CurlyTokenType.DefaultAttributeValue)
                {
                    result.AddRange(attributeNames
                        .Where(n => currentToken.Type == TokenType.Whitespace || n.StartsWith(currentToken.Text))
                        .Select(n => CreateItem(currentToken, n, StandardGlyphGroup.GlyphGroupProperty))
                    );
                }
            }

            CompletionSet newCompletionSet = new CompletionSet(
                Moniker,
                "Neptuo Templates",
                FindTokenSpanAtPosition(session.GetTriggerPoint(textBuffer), session, currentToken),
                result,
                null
            );
            //completionSets.Add(MergeCompletionSets(completionSets, newCompletionSet));

            if (completionSets.Any())
                completionSets.RemoveAt(0);

            newCompletionSet.SelectBestMatch();
            completionSets.Add(newCompletionSet);
        }

        private Completion CreateItem(Token currentToken, string targetValue, StandardGlyphGroup glyphGroup, StandardGlyphItem glyphItem = StandardGlyphItem.GlyphItemPublic)
        {
            return new Completion(
                targetValue, 
                targetValue, 
                "This is such a usefull description for this item.",
                glyphService.GetGlyph(glyphGroup, glyphItem), 
                "Hello, Template!"
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

        private ITrackingSpan FindTokenSpanAtPosition(ITrackingPoint point, ICompletionSession session, Token currentToken)
        {
            int startIndex = currentToken.TextSpan.StartIndex;
            int length = Math.Max(0, currentToken.TextSpan.StartIndex + currentToken.TextSpan.Length - startIndex);
            if (currentToken.Type == CurlyTokenType.OpenBrace || currentToken.Type == CurlyTokenType.Whitespace)
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
    }
}
