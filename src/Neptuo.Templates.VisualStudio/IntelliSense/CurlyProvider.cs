using Microsoft.VisualStudio.Language.Intellisense;
using Neptuo.Templates.Compilation.Parsers.Syntax.Nodes;
using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using Neptuo.Templates.VisualStudio.IntelliSense.Classifications;
using Neptuo.Templates.VisualStudio.IntelliSense.Completions;
using Neptuo.Templates.VisualStudio.IntelliSense.Completions.Sources;
using Neptuo.Templates.VisualStudio.IntelliSense.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.VisualStudio.IntelliSense
{
    /// <summary>
    /// 'Curly' implementation of <see cref="ICompletionTriggerProvider"/> and <see cref="ICompletionProvider"/>.
    /// </summary>
    public class CurlyProvider : ICompletionTriggerProvider, ICompletionProvider, IAutomaticCompletionProvider, ITokenClassificationProvider
    {
        private readonly GlyphHelper glyphs;
        private readonly ICurlyCompletionSource completionSource;
        private readonly Dictionary<TokenType, string> classifications = new Dictionary<TokenType, string>();

        public CurlyProvider(IGlyphService glyphService, ICurlyCompletionSource completionSource)
        {
            Ensure.NotNull(glyphService, "glyphService");
            Ensure.NotNull(completionSource, "completionSource");
            this.glyphs = new GlyphHelper(glyphService);
            this.completionSource = completionSource;
        }

        #region Completions

        public IEnumerable<ITokenTrigger> GetStartTriggers()
        {
            yield return new DefaultTokenTrigger(CurlyTokenType.OpenBrace, false);
            yield return new DefaultTokenTrigger(CurlyTokenType.Name, true);
            yield return new DefaultTokenTrigger(CurlyTokenType.NamePrefix, true);
            yield return new DefaultTokenTrigger(CurlyTokenType.NameSeparator, false);
            yield return new DefaultTokenTrigger(CurlyTokenType.Whitespace, false);
            yield return new DefaultTokenTrigger(CurlyTokenType.DefaultAttributeValue, true);
            yield return new DefaultTokenTrigger(CurlyTokenType.AttributeName, true);
        }

        public IEnumerable<ITokenTrigger> GetCommitTriggers()
        {
            yield return new DefaultTokenTrigger(CurlyTokenType.AttributeValueSeparator, false);
        }

        public IEnumerable<ICompletion> GetCompletions(ISyntaxNode currentNode, Token currentToken)
        {
            List<ICompletion> result = new List<ICompletion>();

            if (currentToken.Type == CurlyTokenType.OpenBrace)
            {
                // Add all withnout filtering.
                result.AddRange(completionSource.GetComponents(null, glyphs.GetExtensionMethod()));
            }

            if (currentToken.Type == CurlyTokenType.Name)
            {
                // Add all matching current node.
                CurlyNameSyntax node = currentNode.FindSelfOrFirstAncestorOfType<CurlyNameSyntax>();
                if (node == null)
                {
                    result.AddRange(completionSource.GetComponents(currentToken.Text, glyphs.GetExtensionMethod()));
                }
                else
                {
                    string name = currentToken.Text;
                    if (node.NameToken != null)
                        name = node.NameToken.Text;

                    string prefix = null;
                    if (node.PrefixToken != null)
                        prefix = node.PrefixToken.Text;

                    result.AddRange(completionSource.GetComponents(prefix, name, glyphs.GetExtensionMethod()));
                }
            }
            else if (currentToken.Type == TokenType.Whitespace || currentToken.Type == CurlyTokenType.AttributeName || currentToken.Type == CurlyTokenType.DefaultAttributeValue)
            {
                // Find current component and offer attributes.
                CurlySyntax node = currentNode.FindFirstAncestorOfType<CurlySyntax>();
                if (node == null)
                    return Enumerable.Empty<ICompletion>();

                string prefix = null;
                if(node.Name.PrefixToken != null)
                    prefix = node.Name.PrefixToken.Text;

                string name = null;
                if(node.Name.NameToken != null)
                    name = node.Name.NameToken.Text;

                if (currentToken.Type == TokenType.Whitespace)
                {
                    // Add all not used attributes (without filtering).
                    result.AddRange(completionSource.GetAttributes(node, null, glyphs.Get(StandardGlyphGroup.GlyphGroupProperty)));
                }
                else
                {
                    // Add all not used matching current text (name).
                    result.AddRange(completionSource.GetAttributes(node, currentToken.Text, glyphs.Get(StandardGlyphGroup.GlyphGroupProperty)));
                }
            }
            else if (currentToken.Type == TokenType.Literal || currentToken.Type == TokenType.Whitespace || currentToken.Type == AngleTokenType.AttributeOpenValue)
            {
                // Add all without filtering + '{' as OpenBrace, because we are at prefix token.
                result.AddRange(completionSource
                    .GetComponents(null, glyphs.GetExtensionMethod())
                    .Select(c => new DefaultCompletion(c.DisplayText, "{" + c.InsertionText, c.DescriptionText, c.IconSource))
                );
            }

            return result;
        }

        #endregion

        #region AutomaticCompletions

        public bool TryGet(TokenCursor cursor, RelativePosition cursorPosition, out IAutomaticCompletion completion)
        {
            if (cursor.Current.Type == CurlyTokenType.OpenBrace)
            {
                TokenCursor nextCursor;
                if (cursor.TryNext(out nextCursor) && (nextCursor.Current.Type != CurlyTokenType.CloseBrace || nextCursor.Current.IsVirtual))
                {
                    completion = new DefaultAutomaticCompletion("}", RelativePosition.Start(), new RelativePosition(-1));
                    return true;
                }
            }

            completion = null;
            return false;
        }

        #endregion

        #region Classification

        public bool TryGet(TokenType tokenType, out string classificationName)
        {
            if (classifications.Count == 0)
            {
                classifications[CurlyTokenType.OpenBrace] = ClassificationType.CurlyBrace;
                classifications[CurlyTokenType.CloseBrace] = ClassificationType.CurlyBrace;
                classifications[CurlyTokenType.AttributeSeparator] = ClassificationType.CurlyBrace;
                classifications[CurlyTokenType.AttributeValueSeparator] = ClassificationType.CurlyBrace;
                classifications[CurlyTokenType.NameSeparator] = ClassificationType.CurlyBrace;

                classifications[CurlyTokenType.Name] = ClassificationType.CurlyName;
                classifications[CurlyTokenType.NamePrefix] = ClassificationType.CurlyName;

                classifications[CurlyTokenType.AttributeName] = ClassificationType.CurlyAttributeName;
            }

            return classifications.TryGetValue(tokenType, out classificationName);
        }

        #endregion
    }
}
