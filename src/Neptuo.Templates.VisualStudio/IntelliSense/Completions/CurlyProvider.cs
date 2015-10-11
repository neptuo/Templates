using Microsoft.VisualStudio.Language.Intellisense;
using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.VisualStudio.IntelliSense.Completions
{
    /// <summary>
    /// 'Curly' implementation of <see cref="ITokenTriggerProvider"/> and <see cref="ICompletionProvider"/>.
    /// </summary>
    public class CurlyProvider : ITokenTriggerProvider, ICompletionProvider
    {
        private readonly List<string> tokenNames = new List<string>() { "Binding", "TemplateBinding", "Template", "Source", "StaticResource" };
        private readonly List<string> attributeNames = new List<string>() { "Path", "Converter", "Key" };
        private readonly IGlyphService glyphService;

        public CurlyProvider(IGlyphService glyphService)
        {
            Ensure.NotNull(glyphService, "glyphService");
            this.glyphService = glyphService;
        }

        public IEnumerable<ITokenTrigger> GetTriggers()
        {
            yield return new DefaultTokenTrigger(CurlyTokenType.OpenBrace, false);
            yield return new DefaultTokenTrigger(CurlyTokenType.Name, true);
            yield return new DefaultTokenTrigger(CurlyTokenType.NamePrefix, true);
            yield return new DefaultTokenTrigger(CurlyTokenType.NameSeparator, false);
            yield return new DefaultTokenTrigger(CurlyTokenType.Whitespace, false);
            yield return new DefaultTokenTrigger(CurlyTokenType.DefaultAttributeValue, true);
            yield return new DefaultTokenTrigger(CurlyTokenType.AttributeName, true);
        }

        public IEnumerable<ICompletion> GetCompletions(IReadOnlyList<Token> tokens, Token currentToken)
        {
            List<ICompletion> result = new List<ICompletion>();

            if (currentToken.Type == CurlyTokenType.Name || currentToken.Type == CurlyTokenType.OpenBrace)
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
    }
}
