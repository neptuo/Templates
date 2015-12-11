using Neptuo.Collections.Specialized;
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
            InternalContext thisContext = new InternalContext(result, decorator, context);
            ReadTokenStart(thisContext);
        }

        private bool ReadTokenStart(InternalContext context)
        {
            if (context.Decorator.Current != '{')
            {
                IList<Token> tokens = context.BuilderContext.TokenizePartial(context.Decorator, '{', '}');
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
            else if(!context.Decorator.IsCurrentEndOfInput())
            {
                context
                    .CreateToken(CurlyTokenType.Literal, -1, true)
                    .WithError("Not valid here.");

                if (context.Decorator.Next())
                    return ReadTokenStart(context);
            }

            return false;
        }

        private void ReadTokenName(InternalContext context, bool isSingleTokenOnly = false)
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
                context.Decorator.Next();

                // Token is closed, so we can end.
                if (isSingleTokenOnly)
                    return;

                // If there is something to read.
                if (context.Decorator.IsCurrentEndOfInput())
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
        private bool ReadTokenAttribute(InternalContext context, bool supportDefaultAttributes = true)
        {
            List<char> specials = new List<char>() { '=', ',', '{', '}' };
            context.Decorator.CurrentUntil(specials.Contains);
            bool result = false;

            if (context.Decorator.Current == '=')
            {
                if (IsValidIdentifier(context.Decorator.CurrentContent(1)))
                {
                    // Use as attribute name.
                    context.CreateToken(CurlyTokenType.AttributeName, 1);
                    context.CreateToken(CurlyTokenType.AttributeValueSeparator);
                    context.Decorator.Next();
                }
                else
                {
                    // Use as error.
                    context
                        .CreateToken(CurlyTokenType.Literal, 1, true)
                        .WithError("Not valid here.");
                }

                // Use as attribute value.
                if (context.Decorator.Current == '{')
                {
                    // Check if current line contains '}' + 1 to '{'.
                    // Read inner token until (excluding) last '}'.
                    int countToRead;
                    if (TryGetNextIndexOfInnerTokenCloseBrace(context.Decorator, out countToRead))
                    {
                        // Parse as inner token.
                        context.Decorator.ResetCurrentPosition(1);
                        countToRead++;
                        IContentReader partialReader = ContentReader.Partial(context.Decorator, c => --countToRead < 0);
                        ContentDecorator partialDecorator = new ContentDecorator(
                            partialReader,
                            context.Decorator.Position,
                            context.Decorator.LineIndex,
                            context.Decorator.ColumnIndex
                        );
                        partialDecorator.Next();

                        IList<Token> innerTokens = Tokenize(partialDecorator, context.BuilderContext);
                        context.Result.AddRange(innerTokens);

                        context.Decorator.ResetCurrentPosition(1);
                        context.Decorator.ResetCurrentInfo();
                        context.Decorator.Next();
                    }
                    else
                    {
                        throw Ensure.Exception.NotSupported();
                    }

                    //context.CreateToken(CurlyTokenType.OpenBrace);
                    //ReadTokenName(context, true);
                }
                else
                {
                    IList<Token> attributeValue = context.BuilderContext.TokenizePartial(context.Decorator, ',', '}');
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
                        context.CreateToken(CurlyTokenType.Whitespace, 1);

                    // Try read next attribute.
                    if (!ReadTokenAttribute(context, false))
                        new TokenFactory(context.Result.Last()).WithError("Missing attribute definition.");
                }

                result = true;
            }
            else
            {
                // Use as default attribute or mark as error.
                if (supportDefaultAttributes)
                {
                    if(context.Decorator.Current == ContentReader.EndOfInput)
                        context.CreateToken(CurlyTokenType.DefaultAttributeValue);
                    else
                        context.CreateToken(CurlyTokenType.DefaultAttributeValue, 1);

                    result = true;
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
                        result = true;
                        supportDefaultAttributes = false;
                    }
                    else if (currentContent != String.Empty)
                    {
                        // Use as error.
                        context
                            .CreateToken(CurlyTokenType.Literal, 1, true)
                            .WithError("Not valid here.");
                    }
                }

                // If separator was found.
                if (context.Decorator.Current == ',')
                {
                    // Use as separator.
                    context.CreateToken(CurlyTokenType.AttributeSeparator);

                    // While whitespaces, read..
                    if (context.Decorator.NextWhile(Char.IsWhiteSpace))
                        context.CreateToken(CurlyTokenType.Whitespace, 1);

                    // Try read next attribute.
                    if (!ReadTokenAttribute(context, supportDefaultAttributes))
                        new TokenFactory(context.Result.Last()).WithError("Missing attribute definition.");
                }
            }

            return result;
        }

        private bool TryGetNextIndexOfInnerTokenCloseBrace(ContentDecorator decorator, out int lastCloseIndex)
        {
            int position = decorator.Position;

            int openCount = 0;
            lastCloseIndex = 0;
            while (decorator.Next())
            {
                if (decorator.Current == '{')
                    openCount++;

                if (decorator.Current == '}')
                {
                    lastCloseIndex = decorator.Position;
                    openCount--;

                    if (openCount == -2)
                        break;
                }

                if (decorator.Current == '\r' || decorator.Current == '\n')
                    break;
            }

            if (openCount != -2)
                lastCloseIndex--;

            decorator.ResetCurrentPositionToIndex(position);
            lastCloseIndex -= (position - 1);
            return true;
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
