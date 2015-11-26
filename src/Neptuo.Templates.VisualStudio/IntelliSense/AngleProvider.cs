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
    /// 'Angle' implementation of <see cref="ITokenTriggerProvider"/> and <see cref="ICompletionProvider"/>.
    /// </summary>
    public class AngleProvider : ITokenTriggerProvider, ICompletionProvider, ITokenClassificationProvider
    {
        private readonly List<string> elementNames = new List<string>() { "Literal" };
        private readonly List<string> attributeNames = new List<string>() { "Text" };
        private readonly IGlyphService glyphService;
        private readonly Dictionary<TokenType, string> classifications = new Dictionary<TokenType, string>();

        public AngleProvider(IGlyphService glyphService)
        {
            Ensure.NotNull(glyphService, "glyphService");
            this.glyphService = glyphService;
        }

        public IEnumerable<ITokenTrigger> GetStartTriggers()
        {
            yield return new DefaultTokenTrigger(AngleTokenType.OpenBrace, false);
            yield return new DefaultTokenTrigger(AngleTokenType.Name, true);
            yield return new DefaultTokenTrigger(AngleTokenType.NamePrefix, true);
            yield return new DefaultTokenTrigger(AngleTokenType.NameSeparator, false);
            yield return new DefaultTokenTrigger(AngleTokenType.Whitespace, false);
            yield return new DefaultTokenTrigger(AngleTokenType.AttributeName, true);
        }

        public IEnumerable<ITokenTrigger> GetCommitTriggers()
        {
            yield return new DefaultTokenTrigger(AngleTokenType.AttributeValueSeparator, false);
        }

        public IEnumerable<ICompletion> GetCompletions(IReadOnlyList<Token> tokens, Token currentToken)
        {
            List<ICompletion> result = new List<ICompletion>();

            if (currentToken.Type == AngleTokenType.Name || currentToken.Type == AngleTokenType.OpenBrace)
            {
                result.AddRange(elementNames
                    .Where(n => currentToken.Type == AngleTokenType.OpenBrace || n.StartsWith(currentToken.Text))
                    .Select(n => CreateItem(currentToken, n, StandardGlyphGroup.GlyphGroupClass))
                );
            }
            else if (currentToken.Type == AngleTokenType.Whitespace || currentToken.Type == AngleTokenType.AttributeName)
            {
                result.AddRange(attributeNames
                    .Where(n => currentToken.Type == TokenType.Whitespace || n.StartsWith(currentToken.Text))
                    .Select(n => CreateItem(currentToken, n, StandardGlyphGroup.GlyphGroupProperty))
                );
            }
            else if (currentToken.Type == TokenType.Literal || currentToken.Type == TokenType.Whitespace)
            {
                result.AddRange(elementNames
                    .Select(n => CreateItem(currentToken, "<" + n, StandardGlyphGroup.GlyphGroupClass))
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

        private const string delimeter = "HTML Tag Delimiter";
        private const string name = "HTML Element Name";
        private const string attributeName = "HTML Attribute Name";
        private const string attributeValueSeparator = "HTML Attribute Value";
        private const string attributeOpenQuotes = "HTML Attribute Value";
        private const string attributeComment = "HTML Comment";

        public bool TryGet(TokenType tokenType, out string classificationName)
        {
            if (classifications.Count == 0)
            {
                classifications[AngleTokenType.OpenBrace] = delimeter;
                classifications[AngleTokenType.CloseBrace] = delimeter;
                classifications[AngleTokenType.NameSeparator] = delimeter;
                classifications[AngleTokenType.AttributeNameSeparator] = delimeter;
                classifications[AngleTokenType.SelfCloseBrace] = delimeter;

                classifications[AngleTokenType.Name] = name;
                classifications[AngleTokenType.NamePrefix] = name;

                classifications[AngleTokenType.AttributeName] = attributeName;
                classifications[AngleTokenType.AttributeNamePrefix] = attributeName;

                classifications[AngleTokenType.AttributeValueSeparator] = attributeValueSeparator;

                classifications[AngleTokenType.AttributeOpenValue] = attributeOpenQuotes;
                classifications[AngleTokenType.AttributeCloseValue] = attributeOpenQuotes;

                classifications[AngleTokenType.OpenComment] = attributeComment;
                classifications[AngleTokenType.CloseComment] = attributeComment;
            }

            return classifications.TryGetValue(tokenType, out classificationName);
        }
    }
}
