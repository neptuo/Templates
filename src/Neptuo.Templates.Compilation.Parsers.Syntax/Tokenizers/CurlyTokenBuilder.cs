﻿using Neptuo.Collections.Specialized;
using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers.ComponentModel;
using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers
{
    public class CurlyTokenBuilder : TokenBuilderBase, ITokenTypeProvider
    {
        public IEnumerable<TokenType> GetSupportedTokenTypes()
        {
            return new List<TokenType>()
            {
                CurlyTokenType.OpenBrace,
                CurlyTokenType.Name,
                
                CurlyTokenType.DefaultAttributeValue,
                CurlyTokenType.AttributeSeparator,
                
                CurlyTokenType.AttributeName,
                CurlyTokenType.AttributeValueSeparator,
                
                CurlyTokenType.CloseBrace,
                
                CurlyTokenType.Whitespace,
                CurlyTokenType.Literal
            };
        }

        protected override void Tokenize(ContentDecorator decorator, ITokenBuilderContext context, List<Token> result)
        {
            CurlyContext thisContext = new CurlyContext(result, decorator, context);
            ReadTokenStart(thisContext);
        }

        private bool ReadTokenStart(CurlyContext context)
        {
            if (context.Decorator.Current != '{')
            {
                IList<Token> tokens = context.TokenizerContext.TokenizePartial(context.Decorator, '{', '}');
                context.Result.AddRange(tokens);
            }

            if (context.Decorator.Current == '{')
            {
                context.Decorator.ResetCurrentPosition(1);
                context.Decorator.ResetCurrentInfo();
                context.Decorator.Next();

                //context.CreateToken(CurlyTokenType.Text, 1);
                context.CreateToken(CurlyTokenType.OpenBrace);
                ReadTokenName(context);
                return true;
            }
            else if(context.Decorator.Current != ContentReader.EndOfInput)
            {
                context
                    .CreateToken(CurlyTokenType.Literal, -1, true)
                    .WithError("Not valid here.");

                if (context.Decorator.Next())
                    return ReadTokenStart(context);
            }

            return false;
        }

        private void ReadTokenName(CurlyContext context, bool isSingleTokenOnly = false)
        {
            context.Decorator.NextWhile(Char.IsLetterOrDigit);

            bool hasName = false;

            if (context.Decorator.Current == '{')
            {
                Token lastToken = context.Result.Last();
                lastToken.Errors.Add(new DefaultErrorMessage("Not valid here"));
                lastToken.IsSkipped = true;
                context.CreateToken(CurlyTokenType.OpenBrace);
                ReadTokenName(context);
                return;
            }

            if (context.Decorator.Current == ':')
            {
                // Use as prefix.
                context.CreateToken(CurlyTokenType.NamePrefix, 1);
                context.Decorator.Next();
                context.CreateToken(CurlyTokenType.NameSeparator);

                context.Decorator.NextWhile(Char.IsLetterOrDigit);
            }

            if (context.Decorator.Current == ' ')
            {
                // Check for valid name.
                if (IsValidIdentifier(context.Decorator.CurrentContent(1)))
                {
                    // Use as name.
                    context.CreateToken(CurlyTokenType.Name, 1);
                }
                else
                {
                    // Use as error
                    context
                        .CreateToken(CurlyTokenType.Literal, 1, true)
                        .WithError("Not valid here.");
                }

                context.Decorator.NextWhile(Char.IsWhiteSpace);
                context.CreateToken(CurlyTokenType.Whitespace, 1);
                context.Decorator.Next();

                hasName = true;
            }

            if (Char.IsLetter(context.Decorator.Current))
            {
                // Attribute or default attribute.
                ReadTokenAttribute(context);
            }

            if (context.Decorator.Current == '}')
            {
                // Check for valid name.
                string currentContent = context.Decorator.CurrentContent(1);
                if (IsValidIdentifier(currentContent))
                {
                    // Use as name.
                    context.CreateToken(CurlyTokenType.Name, 1);
                    context.Decorator.Next();
                }
                else if (currentContent != String.Empty)
                {
                    // Use as error
                    context
                        .CreateToken(CurlyTokenType.Literal, 1, true)
                        .WithError("Not valid here.");
                }

                // Close token.
                context.CreateToken(CurlyTokenType.CloseBrace);

                // Token is closed, so we can end.
                if (isSingleTokenOnly)
                    return;

                // If there is something to read.
                if (context.Decorator.Next())
                {
                    // Read tokens and accept last characters as text.
                    ReadTokenStart(context);
                    //context.CreateToken(CurlyTokenType.Text);
                    context.Decorator.ResetCurrentInfo();
                }
            }
            else if (context.Decorator.Current == ContentReader.EndOfInput)
            {
                // Use as name and close token (virtually).
                if (hasName)
                    context.CreateToken(CurlyTokenType.AttributeName).WithError("Missing attribute value");
                else
                    context.CreateToken(CurlyTokenType.Name);

                context.CreateVirtualToken(CurlyTokenType.CloseBrace, "}");
            }
            else if (context.Decorator.Current == '{')
            {
                if (hasName)
                {
                    context.CreateVirtualToken(CurlyTokenType.CloseBrace, "}");
                }
                else
                {
                    context.Decorator.NextUntil(c => c == '{');
                    context
                        .CreateToken(CurlyTokenType.Literal, 1, true)
                        .WithError("Not valid here.");
                }

                context.CreateToken(CurlyTokenType.OpenBrace);
                ReadTokenName(context);
            }
        }

        /// <summary>
        /// Reads token attribute or default attribute.
        /// </summary>
        private void ReadTokenAttribute(CurlyContext context, bool supportDefaultAttributes = true)
        {
            List<char> specials = new List<char>() { '=', ',', '{', '}' };
            context.Decorator.NextUntil(specials.Contains);

            if (context.Decorator.Current == '=')
            {
                if (IsValidIdentifier(context.Decorator.CurrentContent(1)))
                {
                    // Use as attribute name.
                    context.CreateToken(CurlyTokenType.AttributeName, 1);
                    context.Decorator.Next();
                    context.CreateToken(CurlyTokenType.AttributeValueSeparator);
                }
                else
                {
                    // Use as error.
                    context
                        .CreateToken(CurlyTokenType.Literal, 1, true)
                        .WithError("Not valid here.");
                }

                // Use as attribute value.
                context.Decorator.Next();
                if (context.Decorator.Current == '{')
                {
                    // Parse as inner token.
                    context.CreateToken(CurlyTokenType.OpenBrace);
                    ReadTokenName(context, true);
                    context.Decorator.Next();
                }
                else
                {
                    IList<Token> attributeValue = context.TokenizerContext.TokenizePartial(context.Decorator, ',', '}');
                    context.Result.AddRange(attributeValue);
                    context.Decorator.ResetCurrentPosition(1);
                    context.Decorator.ResetCurrentInfo();
                    context.Decorator.Next();
                }

                if (context.Decorator.Current == ',')
                {
                    // Use as separator.
                    context.CreateToken(CurlyTokenType.AttributeSeparator);

                    // Read all whitespaces.
                    if (context.Decorator.NextWhile(Char.IsWhiteSpace))
                    {
                        context.CreateToken(CurlyTokenType.Whitespace, 1);
                        context.Decorator.Next();
                    }

                    // Try read next attribute.
                    ReadTokenAttribute(context, false);
                }
            }
            else
            {
                // Use as default attribute or mark as error.
                if (supportDefaultAttributes)
                {
                    context.CreateToken(CurlyTokenType.DefaultAttributeValue, 1);
                    context.Decorator.Next();
                }
                else
                {
                    string currentContent = context.Decorator.CurrentContent(1);
                    if (IsValidIdentifier(currentContent))
                    {
                        // Use as attribute name.
                        context.CreateToken(CurlyTokenType.AttributeName, 1);
                        context.CreateVirtualToken(CurlyTokenType.AttributeValueSeparator, "=");
                        context.CreateVirtualToken(CurlyTokenType.Literal, "");
                        context.Decorator.Next();
                    }
                    else if (currentContent != String.Empty)
                    {
                        // Use as error.
                        context
                            .CreateToken(CurlyTokenType.Literal, 1, true)
                            .WithError("Not valid here.");

                        // We are not able to create token attribute, so last separator is wrong.
                        if (context.Decorator.Current == '}' && context.Result.Last().Type == CurlyTokenType.AttributeSeparator)
                        {
                            new TokenFactory(context.Result.Last())
                                .WithError("Not valid here.");
                        }
                    }
                }

                // If separator was found.
                if (context.Decorator.Current == ',')
                {
                    // Use as separator.
                    context.CreateToken(CurlyTokenType.AttributeSeparator);

                    // While whitespaces, read..
                    if (context.Decorator.NextWhile(Char.IsWhiteSpace))
                    {
                        context.CreateToken(CurlyTokenType.Whitespace, 1);
                        context.Decorator.Next();
                    }

                    // Try read next attribute.
                    ReadTokenAttribute(context);
                }
            }
        }

        private bool IsValidIdentifier(string text)
        {
            if (String.IsNullOrEmpty(text))
                return false;

            if (!Char.IsLetter(text[0]))
                return false;

            foreach (char c in text)
            {
                if (!Char.IsLetterOrDigit(c))
                    return false;
            }

            return true;
        }
    }
}
