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
                AngleTokenType.SelfCloseBrace,
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
                IList<Token> tokens = context.TokenizerContext.TokenizePartial(context.Decorator, '<', '>');
                context.Result.AddRange(tokens);
            }

            if (context.Decorator.Current == '<')
            {
                context.Decorator.ResetCurrentPosition(1);
                context.Decorator.ResetCurrentInfo();
                context.Decorator.Next();

                if (!ChooseNodeType(context))
                {
                    if (context.Decorator.CanResetCurrentPosition(1))
                        context.CreateToken(AngleTokenType.Error, 1);

                    return;
                }
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
                context.CreateToken(AngleTokenType.Error, 1);
                return false;
            }

            throw Ensure.Exception.NotImplemented();
        }

        private bool ReadElementName(InternalContext context)
        {
            if (context.Decorator.CurrentWhile(Char.IsLetterOrDigit))
            {
                if (context.Decorator.Current == ':')
                {
                    context.CreateToken(AngleTokenType.NamePrefix, 1);
                    context.CreateToken(AngleTokenType.NameSeparator);
                    if (context.Decorator.NextWhile(Char.IsLetterOrDigit))
                    {
                        context.CreateToken(AngleTokenType.Name, 1);
                    }
                    else
                    {
                        context.CreateToken(AngleTokenType.Error);
                        return false;
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
                if (context.Decorator.Next())
                {
                    if (context.Decorator.Current == '>')
                    {
                        context.CreateToken(AngleTokenType.SelfCloseBrace);
                        return true;
                    }
                    else
                    {
                        context.CreateToken(AngleTokenType.Error);
                        return false;
                    }
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
            else if (context.Decorator.Current == '<')
            {
                context.CreateVirtualToken(AngleTokenType.SelfCloseBrace, "/>");
                return ChooseNodeType(context);
            }
            else if (context.Decorator.Current == ContentReader.EndOfInput)
            {
                context.CreateVirtualToken(AngleTokenType.SelfCloseBrace, "/>");
                return true;
            }

            return false;
        }

        private bool ReadAttributeName(InternalContext context)
        {
            if (context.Decorator.CurrentWhile(Char.IsLetterOrDigit))
            {
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

                if (context.Decorator.CurrentWhile(Char.IsWhiteSpace))
                    context.CreateToken(AngleTokenType.Whitespace, 1);

                if (context.Decorator.Current == '=')
                {
                    context.CreateToken(AngleTokenType.AttributeValueSeparator);
                    if (context.Decorator.Next())
                    {
                        if (context.Decorator.CurrentWhile(Char.IsWhiteSpace))
                            context.CreateToken(AngleTokenType.Whitespace, 1);

                        if (context.Decorator.Current == '"')
                        {
                            if (ReadAttributeValue(context))
                            {
                                context.Decorator.Next();
                                return ReadOpenElementContent(context);
                            }

                            return false;
                        }
                        else if (HasCloseElement(context.Decorator))
                        {
                            context.CreateVirtualToken(AngleTokenType.AttributeOpenValue, "\"");
                            context.CreateVirtualToken(AngleTokenType.AttributeCloseValue, "\"");
                            return ReadOpenElementContent(context);
                        }
                        else
                        {
                            // TODO: Close
                            context.CreateVirtualToken(AngleTokenType.SelfCloseBrace, "/>");
                            return true;
                        }
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
                    else if (HasCloseElement(context.Decorator))
                    {
                        context.CreateVirtualToken(AngleTokenType.AttributeValueSeparator, "=");
                        context.CreateVirtualToken(AngleTokenType.AttributeOpenValue, "\"");
                        context.CreateVirtualToken(AngleTokenType.AttributeCloseValue, "\"");
                        return ReadOpenElementContent(context);
                    }
                    else
                    {
                        // TODO: Close
                        context.CreateVirtualToken(AngleTokenType.SelfCloseBrace, "/>");
                        return true;
                    }
                }
            }

            return false;
        }

        private bool HasCloseElement(ContentDecorator decorator)
        {
            bool result = false;
            int position = decorator.Position;
            if (decorator.NextUntil(c => c == '>' || c == '<'))
                result = decorator.Current == '>';

            decorator.ResetCurrentPosition(decorator.Position - position);
            return result;
        }

        private bool ReadAttributeValue(InternalContext context)
        {
            context.CreateToken(AngleTokenType.AttributeOpenValue);

            context.Decorator.Next();
            IList<Token> attributeValue = context.TokenizerContext.TokenizePartial(context.Decorator, '"');
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
