using Neptuo.Templates.Compilation.Parsers.Tokenizers.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Tokenizers
{
    public class AngleTokenizer : TokenizerBase, IComposableTokenTypeProvider
    {
        public IEnumerable<ComposableTokenType> GetSupportedTokenTypes()
        {
            return new List<ComposableTokenType>()
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

        protected override void Tokenize(ContentDecorator decorator, IComposableTokenizerContext context, List<ComposableToken> result)
        {
            if (decorator.Current != '<' && decorator.Current != ContentReader.EndOfInput)
            {
                IList<ComposableToken> tokens = context.TokenizePartial(decorator, '<', '>');
                result.AddRange(tokens);
            }

            if (decorator.Current == '<')
            {
                decorator.ResetCurrentPosition(1);
                decorator.ResetCurrentInfo();
                decorator.Next();

                ChooseNodeType(decorator, context, result);
            }

            if (decorator.Current == ContentReader.EndOfInput)
                return;

            throw Ensure.Exception.NotImplemented();
        }

        private bool ChooseNodeType(ContentDecorator decorator, IComposableTokenizerContext context, List<ComposableToken> result)
        {
            // <
            if (!decorator.Next())
                return false;

            // <[a-Z]
            if (Char.IsLetter(decorator.Current))
            {
                CreateToken(decorator, result, AngleTokenType.OpenBrace, 1);
                if (ReadElementName(decorator, context, result))
                {
                    decorator.Next();
                    Tokenize(decorator, context, result);
                    return true;
                }
            }
            // <!
            else if (decorator.Current == '!')
            {
                if (decorator.Next())
                {
                    // <!-
                    if (decorator.Current == '-')
                    {
                        // <!--
                        if (decorator.Next() && decorator.Current == '-')
                        {
                            CreateToken(decorator, result, AngleTokenType.OpenComment);
                            ReadComment(decorator, context, result);
                        }
                        else
                        {
                            CreateToken(decorator, result, AngleTokenType.Error);
                            return false;
                        }
                    }
                    // <![a-Z
                    else if(Char.IsLetter(decorator.Current))
                    {
                        CreateToken(decorator, result, AngleTokenType.OpenDirective);
                        return ReadDirectiveName(decorator, context, result);
                    }
                    else
                    {
                        CreateToken(decorator, result, AngleTokenType.Error);
                        return false;
                    }
                }
                else
                {
                    CreateToken(decorator, result, AngleTokenType.Error);
                    return false;
                }
            }
            else
            {
                CreateToken(decorator, result, AngleTokenType.Error);
                return false;
            }

            throw Ensure.Exception.NotImplemented();
        }

        private bool ReadElementName(ContentDecorator decorator, IComposableTokenizerContext context, List<ComposableToken> result)
        {
            if (decorator.CurrentWhile(Char.IsLetterOrDigit))
            {
                if (decorator.Current == ':')
                {
                    CreateToken(decorator, result, AngleTokenType.NamePrefix, 1);
                    CreateToken(decorator, result, AngleTokenType.NameSeparator);
                    if (decorator.NextWhile(Char.IsLetterOrDigit))
                    {
                        CreateToken(decorator, result, AngleTokenType.Name, 1);
                    }
                    else
                    {
                        CreateToken(decorator, result, AngleTokenType.Error);
                        return false;
                    }
                }
                else
                {
                    CreateToken(decorator, result, AngleTokenType.Name, 1);
                }

                return ReadOpenElementContent(decorator, context, result);
            }
            else if (decorator.Current == ContentReader.EndOfInput)
            {
                CreateToken(decorator, result, AngleTokenType.Name);
                return ReadOpenElementContent(decorator, context, result);
            }

            return false;
        }

        private bool ReadOpenElementContent(ContentDecorator decorator, IComposableTokenizerContext context, List<ComposableToken> result)
        {
            if (decorator.CurrentWhile(Char.IsWhiteSpace) && decorator.CanResetCurrentPosition(1))
                CreateToken(decorator, result, AngleTokenType.Whitespace, 1);

            if (decorator.Current == '/')
            {
                if (decorator.Next())
                {
                    if (decorator.Current == '>')
                    {
                        CreateToken(decorator, result, AngleTokenType.SelfCloseBrace);
                        return true;
                    }
                    else
                    {
                        CreateToken(decorator, result, AngleTokenType.Error);
                        return false;
                    }
                }
            }
            else if (decorator.Current == '>')
            {
                CreateToken(decorator, result, AngleTokenType.CloseBrace);
                throw new NotImplementedException();
            }
            else if (Char.IsLetterOrDigit(decorator.Current))
            {
                return ReadAttributeName(decorator, context, result);
            }
            else if (decorator.Current == '<')
            {
                CreateVirtualToken(result, AngleTokenType.SelfCloseBrace, "/>");
                return ChooseNodeType(decorator, context, result);
            }
            else if (decorator.Current == ContentReader.EndOfInput)
            {
                CreateVirtualToken(result, AngleTokenType.SelfCloseBrace, "/>");
                return true;
            }

            return false;
        }

        private bool ReadAttributeName(ContentDecorator decorator, IComposableTokenizerContext context, List<ComposableToken> result)
        {
            if (decorator.CurrentWhile(Char.IsLetterOrDigit))
            {
                if (decorator.Current == ':')
                {
                    CreateToken(decorator, result, AngleTokenType.AttributeNamePrefix, 1);
                    CreateToken(decorator, result, AngleTokenType.AttributeNameSeparator);
                    if (decorator.NextWhile(Char.IsLetterOrDigit))
                    {
                        CreateToken(decorator, result, AngleTokenType.AttributeName, 1);
                    }
                    else
                    {
                        CreateToken(decorator, result, AngleTokenType.Error);
                        return false;
                    }
                }
                else
                {
                    CreateToken(decorator, result, AngleTokenType.AttributeName, 1);
                }

                if (decorator.CurrentWhile(Char.IsWhiteSpace))
                    CreateToken(decorator, result, AngleTokenType.Whitespace, 1);

                if (decorator.Current == '=')
                {
                    CreateToken(decorator, result, AngleTokenType.AttributeValueSeparator);
                    if (decorator.Next())
                    {
                        if (decorator.CurrentWhile(Char.IsWhiteSpace))
                            CreateToken(decorator, result, AngleTokenType.Whitespace, 1);

                        if (decorator.Current == '"')
                        {
                            if (ReadAttributeValue(decorator, context, result))
                            {
                                decorator.Next();
                                return ReadOpenElementContent(decorator, context, result);
                            }

                            return false;
                        }
                        else
                        {
                            CreateVirtualToken(result, AngleTokenType.AttributeOpenValue, "\"");
                            CreateVirtualToken(result, AngleTokenType.AttributeCloseValue, "\"");
                            return ReadOpenElementContent(decorator, context, result);
                        }
                    }
                }
                else
                {
                    if (decorator.Current == '"')
                    {
                        CreateVirtualToken(result, AngleTokenType.AttributeValueSeparator, "=");
                        if (ReadAttributeValue(decorator, context, result))
                        {
                            decorator.Next();
                            return ReadOpenElementContent(decorator, context, result);
                        }

                        return false;
                    }
                    else //if (decorator.Current == '/' || decorator.Current == '>' || decorator.IsCurrentEndOfInput())
                    {
                        CreateVirtualToken(result, AngleTokenType.AttributeValueSeparator, "=");
                        CreateVirtualToken(result, AngleTokenType.AttributeOpenValue, "\"");
                        CreateVirtualToken(result, AngleTokenType.AttributeCloseValue, "\"");
                        return ReadOpenElementContent(decorator, context, result);
                    }
                }
            }

            return false;
        }

        private bool ReadAttributeValue(ContentDecorator decorator, IComposableTokenizerContext context, List<ComposableToken> result)
        {
            CreateToken(decorator, result, AngleTokenType.AttributeOpenValue);

            decorator.Next();
            IList<ComposableToken> attributeValue = context.TokenizePartial(decorator, '"');
            result.AddRange(attributeValue);
            decorator.ResetCurrentPosition(1);
            decorator.ResetCurrentInfo();
            decorator.Next();
            
            CreateToken(decorator, result, AngleTokenType.AttributeCloseValue);
            return true;
        }

        private bool ReadDirectiveName(ContentDecorator decorator, IComposableTokenizerContext context, List<ComposableToken> result)
        {
            throw Ensure.Exception.NotImplemented();
        }

        private bool ReadComment(ContentDecorator decorator, IComposableTokenizerContext context, List<ComposableToken> result)
        {
            throw Ensure.Exception.NotImplemented();
        }
    }
}
