using Neptuo.Collections.Specialized;
using Neptuo.Templates.Compilation.Parsers.Tokenizers.ComponentModel;
using Neptuo.Templates.Compilation.Parsers.Tokenizers.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Tokenizers
{
    public class CurlyTokenizer : IComposableTokenizer, IComposableTokenTypeProvider
    {
        public IEnumerable<ComposableTokenType> GetSupportedTokenTypes()
        {
            return new List<ComposableTokenType>()
            {
                CurlyTokenType.OpenBrace,
                CurlyTokenType.Name,
                
                CurlyTokenType.DefaultAttributeValue,
                CurlyTokenType.AttributeSeparator,
                
                CurlyTokenType.AttributeName,
                CurlyTokenType.AttributeValueSeparator,
                CurlyTokenType.AttributeValue,
                
                CurlyTokenType.CloseBrace,
                
                CurlyTokenType.Error,
                CurlyTokenType.Whitespace,
                CurlyTokenType.Text
            };
        }

        public IList<ComposableToken> Tokenize(ContentDecorator decorator, IComposableTokenizerContext context)
        {
            List<ComposableToken> result = new List<ComposableToken>();
            ReadTokenStart(decorator, context, result);
            return result;
        }

        private bool ReadTokenStart(ContentDecorator decorator, IComposableTokenizerContext context, List<ComposableToken> result)
        {
            if (decorator.Current != '{')
            {
                IList<ComposableToken> tokens = context.TokenizePartial(decorator, '{', '}');
                result.AddRange(tokens);
            }

            if (decorator.Current == '{')
            {
                decorator.ResetCurrentPosition(1);
                decorator.ResetCurrentInfo();
                decorator.Next();

                //CreateToken(decorator, result, CurlyTokenType.Text, 1);
                CreateToken(decorator, result, CurlyTokenType.OpenBrace);
                ReadTokenName(decorator, context, result);
                return true;
            }
            else if(decorator.Current != ContentReader.EndOfInput)
            {
                CreateToken(decorator, result, CurlyTokenType.Error);
                if (decorator.Next())
                    return ReadTokenStart(decorator, context, result);
            }

            return false;
        }

        private void ReadTokenName(ContentDecorator decorator, IComposableTokenizerContext context, List<ComposableToken> result)
        {
            decorator.NextWhile(Char.IsLetterOrDigit);

            bool hasName = false;

            if (decorator.Current == '{')
            {
                result.Last().Type = CurlyTokenType.Error;
                CreateToken(decorator, result, CurlyTokenType.OpenBrace);
                ReadTokenName(decorator, context, result);
                return;
            }

            if (decorator.Current == ':')
            {
                // Use as prefix.
                CreateToken(decorator, result, CurlyTokenType.NamePrefix, 1);
                CreateToken(decorator, result, CurlyTokenType.NameSeparator);

                decorator.NextWhile(Char.IsLetterOrDigit);
            }

            if (decorator.Current == ' ')
            {
                // Check for valid name.
                if (IsValidIdentifier(decorator.CurrentContent(1)))
                {
                    // Use as name.
                    CreateToken(decorator, result, CurlyTokenType.Name, 1);
                }
                else
                {
                    // Use as error
                    CreateToken(decorator, result, CurlyTokenType.Error, 1);
                }

                decorator.NextWhile(Char.IsWhiteSpace);
                CreateToken(decorator, result, CurlyTokenType.Whitespace, 1);

                hasName = true;
            }

            if (Char.IsLetter(decorator.Current))
            {
                // Attribute or default attribute.
                ReadTokenAttribute(decorator, context, result);
            }

            if (decorator.Current == '}')
            {
                // Check for valid name.
                if (IsValidIdentifier(decorator.CurrentContent(1)))
                {
                    // Use as name.
                    CreateToken(decorator, result, CurlyTokenType.Name, 1);
                }
                else
                {
                    // Use as error
                    CreateToken(decorator, result, CurlyTokenType.Error, 1);
                }

                // Close token.
                CreateToken(decorator, result, CurlyTokenType.CloseBrace);

                // If there is something to read.
                if (decorator.Next())
                {
                    // Read tokens and accept last characters as text.
                    ReadTokenStart(decorator, context, result);
                    //CreateToken(decorator, result, CurlyTokenType.Text);
                    decorator.ResetCurrentInfo();
                }
            }
            else if (decorator.Current == ContentReader.EndOfInput)
            {
                // Use as name and close token (virtually).
                CreateToken(decorator, result, CurlyTokenType.Name);
                CreateVirtualToken(result, CurlyTokenType.CloseBrace, "}");
            }
            else if (decorator.Current == '{')
            {
                if (hasName)
                {
                    CreateVirtualToken(result, CurlyTokenType.CloseBrace, "}");
                }
                else
                {
                    decorator.NextUntil(c => c == '{');
                    CreateToken(decorator, result, CurlyTokenType.Error, 1);
                }

                CreateToken(decorator, result, CurlyTokenType.OpenBrace);
                ReadTokenName(decorator, context, result);
            }
        }

        private void ReadTokenAttribute(ContentDecorator decorator, IComposableTokenizerContext context, List<ComposableToken> result, bool supportDefaultAttributes = true)
        {
            List<char> specials = new List<char>() { '=', ',', '{', '}' };
            decorator.NextUntil(specials.Contains);

            if (decorator.Current == '=')
            {
                if (IsValidIdentifier(decorator.CurrentContent(1)))
                {
                    // Use as attribute name.
                    CreateToken(decorator, result, CurlyTokenType.AttributeName, 1);
                    CreateToken(decorator, result, CurlyTokenType.AttributeValueSeparator);
                }
                else
                {
                    // Use as error.
                    CreateToken(decorator, result, CurlyTokenType.Error, 1);
                }


                // Use as attribute value.
                decorator.NextUntil(c => c == ',' || c == '}');
                CreateToken(decorator, result, CurlyTokenType.AttributeValue, 1);

                if (decorator.Current == ',')
                {
                    // Use as separator.
                    CreateToken(decorator, result, CurlyTokenType.AttributeSeparator);

                    // Read all whitespaces.
                    decorator.NextWhile(Char.IsWhiteSpace);
                    CreateToken(decorator, result, CurlyTokenType.Whitespace, 1);

                    // Try read next attribute.
                    ReadTokenAttribute(decorator, context, result, false);
                }
            }
            else
            {
                // Use as default attribute or mark as error.
                if (supportDefaultAttributes)
                {
                    CreateToken(decorator, result, CurlyTokenType.DefaultAttributeValue, 1);
                }
                else
                {
                    if (IsValidIdentifier(decorator.CurrentContent(1)))
                    {
                        // Use as attribute name.
                        CreateToken(decorator, result, CurlyTokenType.AttributeName, 1);
                        result.Last().Error = new DefaultErrorMessage("Missing attribute value.");
                    }
                    else
                    {
                        // Use as error.
                        CreateToken(decorator, result, CurlyTokenType.Error, 1);
                    }
                }

                // If separator was found.
                if (decorator.Current == ',')
                {
                    // Use as separator.
                    CreateToken(decorator, result, CurlyTokenType.AttributeSeparator);

                    // While whitespaces, read..
                    decorator.NextWhile(Char.IsWhiteSpace);
                    CreateToken(decorator, result, CurlyTokenType.Whitespace, 1);

                    // Try read next attribute.
                    ReadTokenAttribute(decorator, context, result);
                }
            }
        }

        private void CreateToken(ContentDecorator decorator, List<ComposableToken> result, ComposableTokenType tokenType, int stepsToGoBack = -1)
        {
            if (stepsToGoBack > 0)
            {
                if (!decorator.ResetCurrentPosition(stepsToGoBack))
                    throw Ensure.Exception.NotSupported("Unnable to process back steps.");
            }

            string text = decorator.CurrentContent();
            if (!String.IsNullOrEmpty(text))
            {
                result.Add(new ComposableToken(tokenType, text)
                {
                    ContentInfo = decorator.CurrentContentInfo(),
                    LineInfo = decorator.CurrentLineInfo(),
                });

                decorator.ResetCurrentInfo();
            }

            if (stepsToGoBack > 0)
                decorator.Read(stepsToGoBack);
        }

        private void CreateVirtualToken(List<ComposableToken> result, ComposableTokenType tokenType, string text)
        {
            result.Add(new ComposableToken(tokenType, text)
            {
                IsVirtual = true
            });
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
