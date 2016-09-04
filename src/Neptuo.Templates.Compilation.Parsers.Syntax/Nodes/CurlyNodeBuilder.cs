using Neptuo.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Nodes
{
    public class CurlyNodeBuilder : INodeBuilder
    {
        public INode Build(TokenReader reader, INodeBuilderContext context)
        {
            CurlyNode result = new CurlyNode();

            Token token = reader.Current;
            while (token.Type == CurlyTokenType.Whitespace)
            {
                result.LeadingTrivia.Add(token);
                if (!reader.Next())
                    throw new MissingNextTokenException();

                token = reader.Current;
            }

            if (token.Type == CurlyTokenType.OpenBrace)
            {
                result.WithOpenToken(token);
                return BuildName(reader, result, context);
            }
            else
            {
                throw new InvalidTokenTypeException(reader.Current, CurlyTokenType.OpenBrace);
            }
        }

        private INode BuildName(TokenReader reader, CurlyNode result, INodeBuilderContext context)
        {
            reader.NextRequired();

            CurlyNameNode name = new CurlyNameNode();
            if (reader.Current.Type == CurlyTokenType.NamePrefix)
            {
                name.WithPrefixToken(reader.Current);

                reader.NextRequiredOfType(CurlyTokenType.NameSeparator);
                name.WithNameSeparatorToken(reader.Current);

                reader.NextRequiredOfType(CurlyTokenType.Name);
                name.WithNameToken(reader.Current);
            }
            else if (reader.Current.Type == CurlyTokenType.Name)
            {
                name.WithNameToken(reader.Current);
            }
            else
            {
                throw new InvalidTokenTypeException(reader.Current, CurlyTokenType.NamePrefix, CurlyTokenType.Name);
            }

            TryAppendTrailingTrivia(reader, name);
            result.WithName(name);
            BuildContent(reader, result, context);
            return result;
        }

        private void BuildContent(TokenReader reader, CurlyNode result, INodeBuilderContext context)
        {
            reader.NextRequired();
            if (reader.Current.Type == CurlyTokenType.CloseBrace)
                BuildTokenClose(reader, result, context);
            else if (reader.Current.Type == CurlyTokenType.AttributeName)
                BuildAttribute(reader, result, context);
            else if (reader.Current.Type == CurlyTokenType.DefaultAttributeValue || reader.Current.Type == CurlyTokenType.Literal)
                BuildDefaultAttribute(reader, result, context);
            else
                throw new InvalidTokenTypeException(reader.Current, CurlyTokenType.CloseBrace, CurlyTokenType.AttributeName, CurlyTokenType.DefaultAttributeValue, CurlyTokenType.Literal);
        }

        private void BuildDefaultAttribute(TokenReader reader, CurlyNode result, INodeBuilderContext context)
        {
            CurlyDefaultAttributeNode attribute = new CurlyDefaultAttributeNode();
            if (reader.Current.Type == CurlyTokenType.DefaultAttributeValue)
                attribute.WithValue(new LiteralNode().WithTextToken(reader.Current));
            else
                attribute.WithValue(context.BuildNext(reader));

            result.AddDefaultAttribute(attribute);

            reader.NextRequired();
            //TODO: Partially copied from BuildAttribute.
            if (reader.Current.Type == CurlyTokenType.CloseBrace)
            {
                TryAppendTrailingTrivia(reader, attribute);
                BuildTokenClose(reader, result, context);
            }
            else if (reader.Current.Type == CurlyTokenType.AttributeSeparator)
            {
                attribute.TrailingTrivia.Add(reader.Current);
                TryAppendTrailingTrivia(reader, attribute);

                reader.NextRequired();
                if (reader.Current.Type == CurlyTokenType.AttributeName)
                    BuildAttribute(reader, result, context);
                else if (reader.Current.Type == CurlyTokenType.DefaultAttributeValue || reader.Current.Type == CurlyTokenType.Literal)
                    BuildDefaultAttribute(reader, result, context);
                else
                    throw new NotImplementedException();
            }
            else if (reader.Current.Type == CurlyTokenType.DefaultAttributeValue || reader.Current.Type == CurlyTokenType.Literal)
            {
                BuildDefaultAttribute(reader, result, context);
            }
            else
            {
                throw new InvalidTokenTypeException(reader.Current, CurlyTokenType.CloseBrace, CurlyTokenType.AttributeSeparator, CurlyTokenType.DefaultAttributeValue, CurlyTokenType.Literal);
            }
        }

        private void BuildAttribute(TokenReader reader, CurlyNode result, INodeBuilderContext context)
        {
            CurlyAttributeNode attribute = new CurlyAttributeNode()
                .WithNameToken(reader.Current);

            result.AddAttribute(attribute);

            reader.NextRequiredOfType(CurlyTokenType.AttributeValueSeparator);
            attribute.WithValueSeparatorToken(reader.Current);

            reader.NextRequired();
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
                attribute.WithValue(context.BuildNext(reader));

                reader.NextRequired();
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
                else if (reader.Current.Type == CurlyTokenType.DefaultAttributeValue || reader.Current.Type == CurlyTokenType.Literal)
                {
                    BuildDefaultAttribute(reader, result, context);
                }
                else
                {
                    throw new InvalidTokenTypeException(reader.Current, CurlyTokenType.CloseBrace, CurlyTokenType.AttributeSeparator, CurlyTokenType.DefaultAttributeValue, CurlyTokenType.Literal);
                }
            }
        }

        private void BuildTokenClose(TokenReader reader, CurlyNode result, INodeBuilderContext context)
        {
            result.WithCloseToken(reader.Current);
        }

        private void TryAppendTrailingTrivia<T>(TokenReader reader, T syntax)
            where T : NodeBase<T>
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
