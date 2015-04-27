using Neptuo.Templates.Compilation.Parsers.Tokenizers.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Tokenizers
{
    public class AngleTokenizer : TokenizerBase
    {
        protected override void Tokenize(ContentDecorator decorator, IComposableTokenizerContext context, List<ComposableToken> result)
        {
            if (decorator.Current != '<')
            {
                IList<ComposableToken> tokens = context.TokenizePartial(decorator, '<', '>');
                result.AddRange(tokens);
            }

            if (decorator.Current == '<')
                ChooseNodeType(decorator, context, result);

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
                CreateToken(decorator, result, AngleTokenType.OpenBrace);
                return ReadElementName(decorator, context, result);
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
                        CreateToken(decorator, result, AngleTokenType.Name);
                    } 
                    else 
                    {
                        CreateToken(decorator, result, AngleTokenType.Error);
                        return false;
                    }
                }

                if (decorator.CurrentWhile(Char.IsWhiteSpace))
                    CreateToken(decorator, result, AngleTokenType.Whitespace);

                if (decorator.Current == '/')
                {
                    if (decorator.Next())
                    {
                        if (decorator.Current == '>')
                        {
                            CreateToken(decorator, result, AngleTokenType.SelfCloseBrace);
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
                    return true;
                }
            }

            return false;
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
