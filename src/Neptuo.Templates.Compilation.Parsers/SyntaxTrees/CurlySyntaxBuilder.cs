﻿using Neptuo.Templates.Compilation.Parsers.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.SyntaxTrees
{
    public class CurlySyntaxBuilder : IComposableSyntaxBuilder
    {
        public ISyntaxNode Build(IList<ComposableToken> tokens, int startIndex, IComposableSyntaxBuilderContext context)
        {
            CurlySyntax result = new CurlySyntax();

            TokenListReader reader = new TokenListReader(tokens, startIndex);
            ComposableToken token = reader.Current;

            while (token.Type == CurlyTokenType.Whitespace)
            {
                result.LeadingTrivia.Add(token);
                if (!reader.Next())
                    throw new NotImplementedException();

                token = reader.Current;
            }

            if (token.Type == CurlyTokenType.OpenBrace)
            {
                result.OpenToken = token;
                return BuildName(reader, result, context);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        private ISyntaxNode BuildName(TokenListReader reader, CurlySyntax result, IComposableSyntaxBuilderContext context)
        {
            reader.NextRequired();

            CurlyNameSyntax name = new CurlyNameSyntax();
            if (reader.Current.Type == CurlyTokenType.NamePrefix)
            {
                name.PrefixToken = reader.Current;

                reader.NextRequiredOfType(CurlyTokenType.NameSeparator);
                name.NameSeparatorToken = reader.Current;

                reader.NextRequiredOfType(CurlyTokenType.Name);
                name.NameToken = reader.Current;
            }
            else if (reader.Current.Type == CurlyTokenType.Name)
            {
                name.NameToken = reader.Current;
            }
            else
            {
                throw new NotImplementedException();
            }

            TryAppendTrailingTrivia(reader, name);
            result.Name = name;
            BuildContent(reader, result, context);
            return result;
        }

        private void BuildContent(TokenListReader reader, CurlySyntax result, IComposableSyntaxBuilderContext context)
        {
            reader.NextRequired();
            if (reader.Current.Type == CurlyTokenType.CloseBrace)
                BuildTokenClose(reader, result, context);
            else if (reader.Current.Type == CurlyTokenType.AttributeName)
                BuildAttribute(reader, result, context);
            else
                throw new NotImplementedException();
        }

        private void BuildAttribute(TokenListReader reader, CurlySyntax result, IComposableSyntaxBuilderContext context)
        {
            CurlyAttributeSyntax attribute = new CurlyAttributeSyntax()
            {
                NameToken = reader.Current
            };
            result.Attributes.Add(attribute);

            reader.NextRequiredOfType(CurlyTokenType.AttributeValueSeparator);
            attribute.ValueSeparatorToken = reader.Current;

            reader.NextRequired();
            attribute.Value = context.BuildNext(reader.Tokens, reader.Position);
            if (attribute.Value != null)
                reader.Next(attribute.Value.GetTokens().Count());

            if (reader.Current.Type == CurlyTokenType.CloseBrace)
            {
                TryAppendTrailingTrivia(reader, attribute);
                BuildTokenClose(reader, result, context);
            }
            else if (reader.Current.Type == CurlyTokenType.AttributeSeparator)
            {
                attribute.TrailingTrivia.Add(reader.Current);
                TryAppendTrailingTrivia(reader, attribute);

                reader.NextRequiredOfType(CurlyTokenType.AttributeName);
                BuildAttribute(reader, result, context);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private void BuildTokenClose(TokenListReader reader, CurlySyntax result, IComposableSyntaxBuilderContext context)
        {
            result.CloseToken = reader.Current;
        }

        private void TryAppendTrailingTrivia<T>(TokenListReader reader, T syntax)
            where T : SyntaxNodeBase<T>
        {
            while (reader.Next())
            {
                if (reader.Current.Type == CurlyTokenType.Whitespace)
                    syntax.TrailingTrivia.Add(reader.Current);
                else
                    break;
            }

            reader.Prev();
        }
    }
}
