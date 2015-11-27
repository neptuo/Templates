using Microsoft.VisualStudio.Language.Intellisense;
using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using Neptuo.Templates.VisualStudio.IntelliSense.Classifications;
using Neptuo.Templates.VisualStudio.IntelliSense.Completions;
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
        private readonly List<string> tokenNames = new List<string>() { "Binding", "TemplateBinding", "Template", "Source", "StaticResource" };
        private readonly List<string> attributeNames = new List<string>() { "Path", "Converter", "Key" };
        private readonly IGlyphService glyphService;
        private readonly Dictionary<TokenType, string> classifications = new Dictionary<TokenType, string>();

        public CurlyProvider(IGlyphService glyphService)
        {
            Ensure.NotNull(glyphService, "glyphService");
            this.glyphService = glyphService;
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

        public IEnumerable<ICompletion> GetCompletions(IReadOnlyList<Token> tokens, Token currentToken)
        {
            List<ICompletion> result = new List<ICompletion>();

            if (currentToken.Type == CurlyTokenType.Name || currentToken.Type == CurlyTokenType.OpenBrace)
            {
                result.AddRange(tokenNames
                    .Where(n => currentToken.Type == CurlyTokenType.OpenBrace || n.StartsWith(currentToken.Text))
                    .Select(n => CreateItem(currentToken, n, StandardGlyphGroup.GlyphExtensionMethod))
                );
            }
            else if (currentToken.Type == TokenType.Whitespace || currentToken.Type == CurlyTokenType.AttributeName || currentToken.Type == CurlyTokenType.DefaultAttributeValue)
            {
                result.AddRange(attributeNames
                    .Where(n => currentToken.Type == TokenType.Whitespace || n.StartsWith(currentToken.Text))
                    .Select(n => CreateItem(currentToken, n, StandardGlyphGroup.GlyphGroupProperty))
                );
            }
            else if (currentToken.Type == TokenType.Literal || currentToken.Type == TokenType.Whitespace || currentToken.Type == AngleTokenType.AttributeOpenValue)
            {
                result.AddRange(tokenNames
                    .Select(n => CreateItem(currentToken, "{" + n, StandardGlyphGroup.GlyphExtensionMethod))
                );
            }

            return result;
        }

        private ICompletion CreateItem(Token currentToken, string targetValue, StandardGlyphGroup glyphGroup, StandardGlyphItem glyphItem = StandardGlyphItem.GlyphItemPublic)
        {
            return new DefaultCompletion(
                targetValue,
                "This is such a usefull description for this item.",
                glyphService.GetGlyph(glyphGroup, glyphItem)
            );
        }

        #endregion

        #region AutomaticCompletions

        public bool TryGet(Token currentToken, RelativePosition cursorPosition, out IAutomaticCompletion completion)
        {
            if (currentToken.Type == CurlyTokenType.OpenBrace)
            {
                completion = new DefaultAutomaticCompletion("}", RelativePosition.Start(), new RelativePosition(-1));
                return true;
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
