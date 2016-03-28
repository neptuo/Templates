using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Nodes
{
    public class AngleNodeBuilder : INodeBuilder
    {
        public INode Build(TokenReader reader, INodeBuilderContext context)
        {
            AngleNode result = new AngleNode();

            Token token = reader.Current;
            while (token.Type == AngleTokenType.Whitespace)
            {
                result.LeadingTrivia.Add(token);
                if (!reader.Next())
                    throw new MissingNextTokenException();

                token = reader.Current;
            }

            if (token.Type == AngleTokenType.OpenBrace)
            {
                result.WithOpenToken(token);
                return BuildName(reader, result, context);
            }
            else
            {
                throw new InvalidTokenTypeException(reader.Current, AngleTokenType.OpenBrace);
            }
        }

        private INode BuildName(TokenReader reader, AngleNode result, INodeBuilderContext context)
        {
            reader.NextRequired();
            AngleNameNode name = GetName(reader);

            TryAppendTrailingTrivia(reader, name);
            result.WithName(name);
            BuildContent(reader, result, context);
            return result;
        }

        private AngleNameNode GetName(TokenReader reader)
        {
            AngleNameNode name = new AngleNameNode();
            if (reader.Current.Type == AngleTokenType.NamePrefix)
            {
                name.WithPrefixToken(reader.Current);

                reader.NextRequiredOfType(AngleTokenType.NameSeparator);
                name.WithNameSeparatorToken(reader.Current);

                reader.NextRequiredOfType(AngleTokenType.Name);
                name.WithNameToken(reader.Current);
            }
            else if (reader.Current.Type == AngleTokenType.Name)
            {
                name.WithNameToken(reader.Current);
            }
            else
            {
                throw new InvalidTokenTypeException(reader.Current, AngleTokenType.NamePrefix, AngleTokenType.Name);
            }

            return name;
        }

        private AngleNameNode GetAttributeName(TokenReader reader)
        {
            AngleNameNode name = new AngleNameNode();
            if (reader.Current.Type == AngleTokenType.AttributeNamePrefix)
            {
                name.WithPrefixToken(reader.Current);

                reader.NextRequiredOfType(AngleTokenType.AttributeNameSeparator);
                name.WithNameSeparatorToken(reader.Current);

                reader.NextRequiredOfType(AngleTokenType.AttributeName);
                name.WithNameToken(reader.Current);
            }
            else if (reader.Current.Type == AngleTokenType.AttributeName)
            {
                name.WithNameToken(reader.Current);
            }
            else
            {
                throw new InvalidTokenTypeException(reader.Current, AngleTokenType.AttributeNamePrefix, AngleTokenType.AttributeName);
            }

            return name;
        }

        private void BuildContent(TokenReader reader, AngleNode result, INodeBuilderContext context)
        {
            reader.NextRequired();
            if (reader.Current.Type == AngleTokenType.SelfClose)
                BuildSelfClose(reader, result, context);
            else if (reader.Current.Type == AngleTokenType.AttributeName || reader.Current.Type == AngleTokenType.AttributeNamePrefix)
                BuildAttribute(reader, result, context);
            else
                throw new InvalidTokenTypeException(reader.Current, AngleTokenType.SelfClose, AngleTokenType.AttributeNamePrefix, AngleTokenType.AttributeName);
        }

        private void BuildAttribute(TokenReader reader, AngleNode result, INodeBuilderContext context)
        {
            AngleAttributeNode attribute = new AngleAttributeNode()
                .WithName(GetAttributeName(reader));

            result.AddAttribute(attribute);

            reader.NextRequiredOfType(AngleTokenType.AttributeValueSeparator);
            attribute.WithValueSeparatorToken(reader.Current);

            reader.NextRequiredOfType(AngleTokenType.AttributeOpenValue);
            attribute.WithAttributeOpenValueToken(reader.Current);

            reader.NextRequired();
            if (reader.IsCurrentOfType(AngleTokenType.AttributeCloseValue))
            {
                attribute.WithAttributeCloseValueToken(reader.Current);
            }
            else
            {
                attribute.WithValue(context.BuildNext(reader));

                reader.NextRequiredOfType(AngleTokenType.AttributeCloseValue);
                attribute.WithAttributeCloseValueToken(reader.Current);
            }

            TryAppendTrailingTrivia(reader, attribute);
            BuildContent(reader, result, context);
        }

        private void BuildSelfClose(TokenReader reader, AngleNode result, INodeBuilderContext context)
        {
            result.WithSelfCloseToken(reader.Current);
            reader.NextRequiredOfType(AngleTokenType.CloseBrace);
            result.WithCloseToken(reader.Current);
        }

        private void TryAppendTrailingTrivia<T>(TokenReader reader, T syntax)
            where T : NodeBase<T>
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
