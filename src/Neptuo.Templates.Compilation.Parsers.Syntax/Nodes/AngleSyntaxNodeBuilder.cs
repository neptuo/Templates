using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Nodes
{
    public class AngleSyntaxNodeBuilder : ISyntaxNodeBuilder
    {
        public ISyntaxNode Build(IList<Token> tokens, int startIndex, ISyntaxNodeBuilderContext context)
        {
            AngleSyntax result = new AngleSyntax();

            TokenListReader reader = new TokenListReader(tokens, startIndex);
            Token token = reader.Current;

            while (token.Type == AngleTokenType.Whitespace)
            {
                result.LeadingTrivia.Add(token);
                if (!reader.Next())
                    throw new NotImplementedException();

                token = reader.Current;
            }

            if (token.Type == AngleTokenType.OpenBrace)
            {
                result.OpenToken = token;
                return BuildName(reader, result, context);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        private ISyntaxNode BuildName(TokenListReader reader, AngleSyntax result, ISyntaxNodeBuilderContext context)
        {
            reader.NextRequired();
            AngleNameSyntax name = GetName(reader);

            TryAppendTrailingTrivia(reader, name);
            result.Name = name;
            BuildContent(reader, result, context);
            return result;
        }

        private AngleNameSyntax GetName(TokenListReader reader)
        {
            AngleNameSyntax name = new AngleNameSyntax();
            if (reader.Current.Type == AngleTokenType.NamePrefix)
            {
                name.PrefixToken = reader.Current;

                reader.NextRequiredOfType(AngleTokenType.NameSeparator);
                name.NameSeparatorToken = reader.Current;

                reader.NextRequiredOfType(AngleTokenType.Name);
                name.NameToken = reader.Current;
            }
            else if (reader.Current.Type == AngleTokenType.Name)
            {
                name.NameToken = reader.Current;
            }
            else
            {
                throw new NotImplementedException();
            }

            return name;
        }

        private AngleNameSyntax GetAttributeName(TokenListReader reader)
        {
            AngleNameSyntax name = new AngleNameSyntax();
            if (reader.Current.Type == AngleTokenType.AttributeNamePrefix)
            {
                name.PrefixToken = reader.Current;

                reader.NextRequiredOfType(AngleTokenType.AttributeNameSeparator);
                name.NameSeparatorToken = reader.Current;

                reader.NextRequiredOfType(AngleTokenType.AttributeName);
                name.NameToken = reader.Current;
            }
            else if (reader.Current.Type == AngleTokenType.AttributeName)
            {
                name.NameToken = reader.Current;
            }
            else
            {
                throw new NotImplementedException();
            }

            return name;
        }

        private void BuildContent(TokenListReader reader, AngleSyntax result, ISyntaxNodeBuilderContext context)
        {
            reader.NextRequired();
            if (reader.Current.Type == AngleTokenType.SelfClose)
                BuildSelfClose(reader, result, context);
            else if (reader.Current.Type == AngleTokenType.AttributeName)
                BuildAttribute(reader, result, context);
            else
                throw new NotImplementedException();
        }

        private void BuildAttribute(TokenListReader reader, AngleSyntax result, ISyntaxNodeBuilderContext context)
        {
            AngleAttributeSyntax attribute = new AngleAttributeSyntax()
            {
                Name = GetAttributeName(reader)
            };
            result.Attributes.Add(attribute);

            reader.NextRequiredOfType(AngleTokenType.AttributeValueSeparator);
            attribute.ValueSeparatorToken = reader.Current;

            reader.NextRequiredOfType(AngleTokenType.AttributeOpenValue);
            attribute.AttributeOpenValueToken = reader.Current;

            reader.NextRequired();
            attribute.Value = context.BuildNext(reader.Tokens, reader.Position);
            if (attribute.Value != null)
                reader.Next(attribute.Value.GetTokens().Count());

            reader.CurrentRequiredOfType(AngleTokenType.AttributeCloseValue);
            attribute.AttributeCloseValueToken = reader.Current;

            TryAppendTrailingTrivia(reader, attribute);
            reader.NextRequired();
            if (reader.Current.Type == AngleTokenType.SelfClose)
            {
                BuildSelfClose(reader, result, context);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private void BuildSelfClose(TokenListReader reader, AngleSyntax result, ISyntaxNodeBuilderContext context)
        {
            result.SelfCloseToken = reader.Current;
            reader.NextRequiredOfType(AngleTokenType.CloseBrace);
            result.CloseToken = reader.Current;
        }

        private void TryAppendTrailingTrivia<T>(TokenListReader reader, T syntax)
            where T : SyntaxNodeBase<T>
        {
            while (reader.Next())
            {
                if (reader.Current.Type == AngleTokenType.Whitespace)
                    syntax.TrailingTrivia.Add(reader.Current);
                else
                    break;
            }

            reader.Prev();
        }
    }
}
