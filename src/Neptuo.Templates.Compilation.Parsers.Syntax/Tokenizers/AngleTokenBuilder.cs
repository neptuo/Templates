using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers
{
    public class AngleTokenBuilder : TokenBuilderBase, ITokenTypeProvider
    {
        public IEnumerable<TokenType> GetSupportedTokenTypes()
        {
            return new List<TokenType>()
            {
                AngleTokenType.OpenBrace,
                AngleTokenType.NamePrefix,
                AngleTokenType.NameSeparator,
                AngleTokenType.Name,
                AngleTokenType.SelfClose,
                AngleTokenType.CloseBrace,
                
                AngleTokenType.AttributeNamePrefix,
                AngleTokenType.AttributeNameSeparator,
                AngleTokenType.AttributeName,
                AngleTokenType.AttributeValueSeparator,
                AngleTokenType.AttributeOpenValue,
                AngleTokenType.AttributeCloseValue,
                
                AngleTokenType.OpenDirective,
                
                AngleTokenType.OpenComment,
                AngleTokenType.CloseComment
            };
        }

        

        protected override void Tokenize(ContentDecorator decorator, ITokenBuilderContext context, List<Token> result)
        {
            InternalContext thisContext = new InternalContext(result, decorator, context);
            Tokenize(thisContext);
        }

        private void Tokenize(InternalContext context)
        {
            while (context.Decorator.Current != '<' && context.Decorator.Current != ContentReader.EndOfInput)
            {
                IList<Token> tokens = context.BuilderContext.TokenizePartial(context.Decorator, '<', '>');
                context.Result.AddRange(tokens);

                if (tokens.Count == 0)
                {
                    if (context.Decorator.Current == '>')
                    {
                        context
                            .CreateToken(AngleTokenType.Literal)
                            .WithError("Not supported '>' in text. Encode it as HTML entity.");

                        context.Decorator.Next();
                    }
                }
            }

            if (context.Decorator.Current == '<')
            {
                context.Decorator.ResetCurrentPosition(1);
                context.Decorator.ResetCurrentInfo();
                context.Decorator.Next();

                if (!ChooseNodeType(context))
                    Tokenize(context);
            }

            if (context.Decorator.Current == ContentReader.EndOfInput)
                return;

            throw Ensure.Exception.NotImplemented();
        }

        private bool ChooseNodeType(InternalContext context)
        {
            // <
            if (!context.Decorator.Next())
                return false;

            // <[a-Z]
            if (Char.IsLetter(context.Decorator.Current))
            {
                context.CreateToken(AngleTokenType.OpenBrace, 1);
                if (ReadElementName(context))
                {
                    context.Decorator.Next();
                    Tokenize(context);
                    return true;
                }
            }
            // <!
            else if (context.Decorator.Current == '!')
            {
                if (context.Decorator.Next())
                {
                    // <!-
                    if (context.Decorator.Current == '-')
                    {
                        // <!--
                        if (context.Decorator.Next() && context.Decorator.Current == '-')
                        {
                            context.CreateToken(AngleTokenType.OpenComment);
                            ReadComment(context);
                        }
                        else
                        {
                            context.CreateToken(AngleTokenType.Error);
                            return false;
                        }
                    }
                    // <![a-Z
                    else if (Char.IsLetter(context.Decorator.Current))
                    {
                        context.CreateToken(AngleTokenType.OpenDirective);
                        return ReadDirectiveName(context);
                    }
                    else
                    {
                        context.CreateToken(AngleTokenType.Error);
                        return false;
                    }
                }
                else
                {
                    context.CreateToken(AngleTokenType.Error);
                    return false;
                }
            }
            else
            {
                context
                    .CreateToken(AngleTokenType.OpenBrace, 1)
                    .WithError("Missing tag name.");

                return false;
            }

            context
                .CreateToken(AngleTokenType.Literal)
                .WithError("Not supported '<' in text. Encode it as HTML entity or complete tag.");

            return false;
        }

        private bool ReadElementName(InternalContext context)
        {
            if (context.Decorator.CurrentWhile(Char.IsLetterOrDigit))
            {
                if (context.Decorator.Current == ':')
                {
                    context.CreateToken(AngleTokenType.NamePrefix, 1);
                    context.CreateToken(AngleTokenType.NameSeparator);
                    context.Decorator.NextWhile(Char.IsLetterOrDigit);
                    if (context.Decorator.Current == ' ')
                    {
                        context.CreateToken(AngleTokenType.Name, 1);
                    }
                    else if (context.Decorator.Current == ContentReader.EndOfInput)
                    {
                        if (context.Decorator.CurrentContent() == String.Empty)
                            context.CreateVirtualToken(AngleTokenType.Name, "").WithError("Missing element name");
                        else
                            context.CreateToken(AngleTokenType.Name);
                    }
                    else
                    {
                        if (context.Decorator.CurrentContent().Length == 0)
                        {
                            context
                                .CreateVirtualToken(AngleTokenType.Name, "")
                                .WithError("Missing element name after prefix");

                            return ReadOpenElementContent(context);
                        }
                        else
                        {
                            context
                                .CreateToken(AngleTokenType.Error);

                            return false;
                        }
                    }
                }
                else
                {
                    context.CreateToken(AngleTokenType.Name, 1);
                }

                return ReadOpenElementContent(context);
            }
            else if (context.Decorator.Current == ContentReader.EndOfInput)
            {
                context.CreateToken(AngleTokenType.Name);
                return ReadOpenElementContent(context);
            }

            return false;
        }

        private bool ReadOpenElementContent(InternalContext context)
        {
            if (context.Decorator.CurrentWhile(Char.IsWhiteSpace) && context.Decorator.CanResetCurrentPosition(1))
                context.CreateToken(AngleTokenType.Whitespace, 1);

            if (context.Decorator.Current == '/')
            {
                context.CreateToken(AngleTokenType.SelfClose);
                if (context.Decorator.Next())
                {
                    if (context.Decorator.Current == '>')
                    {
                        context.CreateToken(AngleTokenType.CloseBrace);
                        return true;
                    }
                    else
                    {
                        new TokenFactory(context.Result.Last()).WithError("Missing close brace '>'.");
                        return true;
                    }
                }
                else
                {
                    context.CreateVirtualToken(AngleTokenType.CloseBrace, ">");
                    return true;
                }
            }
            else if (context.Decorator.Current == '>')
            {
                context.CreateToken(AngleTokenType.CloseBrace);
                throw new NotImplementedException();
            }
            else if (Char.IsLetterOrDigit(context.Decorator.Current))
            {
                return ReadAttributeName(context);
            }
            else if (context.Decorator.IsCurrentContained('<', ContentReader.EndOfInput))
            {
                context
                    .CreateVirtualToken(AngleTokenType.SelfClose, "/")
                    .WithError("Missing element close sequence '/>'.");

                context
                    .CreateVirtualToken(AngleTokenType.CloseBrace, ">")
                    .WithError("Missing element close sequence '/>'.");

                if (context.Decorator.Current == '<')
                    return ChooseNodeType(context);
                else if (context.Decorator.Current == ContentReader.EndOfInput)
                    return true;
                else
                    throw Ensure.Exception.NotImplemented("Missing IF for input '{0}'.", context.Decorator.Current);
            }

            return false;
        }

        private bool ReadAttributeName(InternalContext context)
        {
            if (context.Decorator.CurrentWhile(Char.IsLetterOrDigit))
            {
                if (!HasAttributeValue(context.Decorator) && !HasCloseOpeningElement(context.Decorator))
                {
                    context.Decorator.ResetCurrentPosition(context.Decorator.CurrentContent().Length);
                    context.CreateVirtualToken(AngleTokenType.SelfClose, "/");
                    context.CreateVirtualToken(AngleTokenType.CloseBrace, ">");
                    return true;
                }

                if (context.Decorator.Current == ':')
                {
                    context.CreateToken(AngleTokenType.AttributeNamePrefix, 1);
                    context.CreateToken(AngleTokenType.AttributeNameSeparator);
                    if (context.Decorator.NextWhile(Char.IsLetterOrDigit))
                    {
                        context.CreateToken(AngleTokenType.AttributeName, 1);
                    }
                    else
                    {
                        context.CreateToken(AngleTokenType.Error);
                        return false;
                    }
                }
                else
                {
                    context.CreateToken(AngleTokenType.AttributeName, 1);
                }
                
                // TODO: Remove support for spaces before attribute value.
                //if (context.Decorator.CurrentWhile(Char.IsWhiteSpace))
                //    context.CreateToken(AngleTokenType.Whitespace, 1);

                if (context.Decorator.Current == '=')
                {
                    context.CreateToken(AngleTokenType.AttributeValueSeparator);
                    if (context.Decorator.Next())
                    {
                        // TODO: Remove support for spaces before attribute value.
                        //if (context.Decorator.CurrentWhile(Char.IsWhiteSpace))
                        //    context.CreateToken(AngleTokenType.Whitespace, 1);

                        if (context.Decorator.Current == '"')
                        {
                            if (ReadAttributeValue(context))
                            {
                                context.Decorator.Next();
                                return ReadOpenElementContent(context);
                            }

                            return false;
                        }
                        else if (HasCloseOpeningElement(context.Decorator))
                        {
                            context.CreateVirtualToken(AngleTokenType.AttributeOpenValue, "\"");
                            context.CreateVirtualToken(AngleTokenType.AttributeCloseValue, "\"");
                            return ReadOpenElementContent(context);
                        }
                        else
                        {
                            // TODO: Close
                            context.CreateVirtualToken(AngleTokenType.SelfClose, "/");
                            context.CreateVirtualToken(AngleTokenType.CloseBrace, ">");
                            return true;
                        }
                    }
                    else
                    {
                        context.CreateVirtualToken(AngleTokenType.AttributeOpenValue, "\"");
                        context.CreateVirtualToken(AngleTokenType.AttributeCloseValue, "\"");
                        context.CreateVirtualToken(AngleTokenType.SelfClose, "/");
                        context.CreateVirtualToken(AngleTokenType.CloseBrace, ">");
                        return true;
                    }
                }
                else
                {
                    if (context.Decorator.Current == '"')
                    {
                        context.CreateVirtualToken(AngleTokenType.AttributeValueSeparator, "=");
                        if (ReadAttributeValue(context))
                        {
                            context.Decorator.Next();
                            return ReadOpenElementContent(context);
                        }

                        return false;
                    }
                    else if (HasCloseOpeningElement(context.Decorator))
                    {
                        context.CreateVirtualToken(AngleTokenType.AttributeValueSeparator, "=");
                        context.CreateVirtualToken(AngleTokenType.AttributeOpenValue, "\"");
                        context.CreateVirtualToken(AngleTokenType.AttributeCloseValue, "\"");
                        return ReadOpenElementContent(context);
                    }
                    else
                    {
                        // TODO: Close
                        context.CreateVirtualToken(AngleTokenType.SelfClose, "/");
                        context.CreateVirtualToken(AngleTokenType.CloseBrace, ">");
                        return true;
                    }
                }
            }

            return false;
        }

        private bool HasCloseOpeningElement(ContentDecorator decorator)
        {
            bool result = false;
            int position = decorator.Position;
            if (decorator.NextUntil(c => c == '>' || c == '<'))
                result = decorator.Current == '>';

            decorator.ResetCurrentPositionToIndex(position);
            return result;
        }

        private bool HasAttributeValue(ContentDecorator decorator)
        {
            bool result = false;
            int position = decorator.Position;
            if (decorator.CurrentUntil(c => !Char.IsLetterOrDigit(c) || c == '=' || c == '>' || c == '<'))
                result = decorator.Current == '=';

            decorator.ResetCurrentPositionToIndex(position);
            return result;
        }

        private bool ReadAttributeValue(InternalContext context)
        {
            context.CreateToken(AngleTokenType.AttributeOpenValue);

            context.Decorator.Next();
            IList<Token> attributeValue = context.BuilderContext.TokenizePartial(context.Decorator, '"', '\r', '\n');
            context.Result.AddRange(attributeValue);
            context.Decorator.ResetCurrentPosition(1);
            context.Decorator.ResetCurrentInfo();
            context.Decorator.Next();

            context.CreateToken(AngleTokenType.AttributeCloseValue);
            return true;
        }

        private bool ReadDirectiveName(InternalContext context)
        {
            throw Ensure.Exception.NotImplemented();
        }

        private bool ReadComment(InternalContext context)
        {
            throw Ensure.Exception.NotImplemented();
        }
    }
}
