using Neptuo.Collections.Specialized;
using Neptuo.Templates.Compilation.Parsers.Tokenizers.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Tokenizers
{
    public class CurlyComposableTokenizer : IComposableTokenizer, IComposableTokenTypeProvider
    {
        private enum State
        {
            Initial,

            /// <summary>
            /// '{'
            /// </summary>
            OpenBrace,
            Name,

            AttributeName,

            DefaultAttributeValue,

            /// <summary>
            /// '='
            /// </summary>
            AttributeValueSeparator,
            AttributeValue,

            /// <summary>
            /// ','
            /// </summary>
            AttributeSeparator,

            /// <summary>
            /// '}'
            /// </summary>
            CloseBrace
        }

        public class TokenType : ComposableTokenType.TokenType
        {
            public static readonly ComposableTokenType OpenBrace = new ComposableTokenType("Curly.OpenBrace");
            public static readonly ComposableTokenType NamePrefix = new ComposableTokenType("Curly.NamePrefix");
            public static readonly ComposableTokenType NameSeparator = new ComposableTokenType("Curly.NameSeparator");
            public static readonly ComposableTokenType Name = new ComposableTokenType("Curly.Name");

            public static readonly ComposableTokenType DefaultAttributeValue = new ComposableTokenType("Curly.DefaultAttributeName");
            public static readonly ComposableTokenType AttributeSeparator = new ComposableTokenType("Curly.AttributeSeparator");

            public static readonly ComposableTokenType AttributeName = new ComposableTokenType("Curly.AttributeName");
            public static readonly ComposableTokenType AttributeValueSeparator = new ComposableTokenType("Curly.AttributeValueSeparator");
            public static readonly ComposableTokenType AttributeValue = new ComposableTokenType("Curly.AttributeValue");

            public static readonly ComposableTokenType CloseBrace = new ComposableTokenType("Curly.CloseBrace");
        }

        public IEnumerable<ComposableTokenType> GetSupportedTokenTypes()
        {
            return new List<ComposableTokenType>()
            {
                TokenType.OpenBrace,
                TokenType.Name,

                TokenType.DefaultAttributeValue,
                TokenType.AttributeSeparator,

                TokenType.AttributeName,
                TokenType.AttributeValueSeparator,
                TokenType.AttributeValue,

                TokenType.CloseBrace,

                TokenType.Error,
                TokenType.Whitespace,
                TokenType.Text
            };
        }

        public IList<ComposableToken> Tokenize(IContentReader reader, IComposableTokenizerContext context)
        {
            List<ComposableToken> result = new List<ComposableToken>();
            ContentDecorator decorator = new ContentDecorator(reader);
            ReadTokenStart(decorator, context, result);
            return result;
        }

        private bool ReadTokenStart(ContentDecorator decorator, IComposableTokenizerContext context, List<ComposableToken> result)
        {
            if (decorator.ReadUntil(c => c == '{'))
            {
                CreateToken(decorator, result, TokenType.Text, 1);
                CreateToken(decorator, result, TokenType.OpenBrace);
                ReadTokenName(decorator, context, result);
                return true;
            }

            return false;
        }

        private void ReadTokenName(ContentDecorator decorator, IComposableTokenizerContext context, List<ComposableToken> result)
        {
            decorator.ReadWhile(Char.IsLetterOrDigit);

            if (decorator.Current == ':')
            {
                // Use as prefix.
                CreateToken(decorator, result, TokenType.NamePrefix, 1);
                CreateToken(decorator, result, TokenType.NameSeparator);

                decorator.ReadWhile(Char.IsLetterOrDigit);
            } 
            
            if(decorator.Current == ' ')
            {
                // Use as name.
                CreateToken(decorator, result, TokenType.Name, 1);

                decorator.ReadWhile(Char.IsWhiteSpace);
                CreateToken(decorator, result, TokenType.Whitespace, 1);
            }

            if (Char.IsLetter(decorator.Current))
            {
                // Attribute or default attribute.
                ReadTokenAttribute(decorator, context, result);
            }

            if (decorator.Current == '}')
            {
                // Use as name and close token.
                CreateToken(decorator, result, TokenType.Name, 1);
                CreateToken(decorator, result, TokenType.CloseBrace);
                
                // Read tokens and accept last characters as text.
                ReadTokenStart(decorator, context, result);
                CreateToken(decorator, result, TokenType.Text);
            }

            // TODO: This use case is more complicated!
            //if (decorator.Current == StringContentReader.NullChar)
            //{
            //    // Use as name and close token (virtually).
            //    CreateToken(decorator, result, TokenType.Name);
            //    CreateVirtualToken(result, TokenType.CloseBrace, "}");
            //}
        }

        private void ReadTokenAttribute(ContentDecorator decorator, IComposableTokenizerContext context, List<ComposableToken> result, bool supportDefaultAttributes = true)
        {
            decorator.ReadWhile(Char.IsLetterOrDigit);

            if (decorator.Current == '=')
            {
                // Use as attribute name.
                CreateToken(decorator, result, TokenType.AttributeName, 1);
                CreateToken(decorator, result, TokenType.AttributeValueSeparator);

                // Use as attribute value.
                decorator.ReadUntil(c => c == ',' || c == '}');
                CreateToken(decorator, result, TokenType.AttributeValue, 1);

                if (decorator.Current == ',')
                {
                    // Use as separator.
                    CreateToken(decorator, result, TokenType.AttributeSeparator);

                    // Read all whitespaces.
                    decorator.ReadWhile(Char.IsWhiteSpace);
                    CreateToken(decorator, result, TokenType.Whitespace, 1);

                    // Try read next attribute.
                    ReadTokenAttribute(decorator, context, result, false);
                }
            }
            else if (decorator.Current == ',')
            {
                // Use as default attribute or mark as error.
                if (supportDefaultAttributes)
                    CreateToken(decorator, result, TokenType.DefaultAttributeValue, 1);
                else
                    CreateToken(decorator, result, TokenType.Error, 1);

                // Use as separator.
                CreateToken(decorator, result, TokenType.AttributeSeparator);

                // Try read next attribute.
                ReadTokenAttribute(decorator, context, result);
            }
        }

        private void CreateToken(ContentDecorator decorator, List<ComposableToken> result, ComposableTokenType tokenType, int stepsToGoBack = -1)
        {
            if (stepsToGoBack > 0)
                decorator.ResetCurrentPosition(stepsToGoBack);

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
    }
}
