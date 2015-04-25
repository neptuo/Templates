using Neptuo.Collections.Specialized;
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
                CreateToken(decorator, result, CurlyTokenType.Text, 1);
                CreateToken(decorator, result, CurlyTokenType.OpenBrace);
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
                CreateToken(decorator, result, CurlyTokenType.NamePrefix, 1);
                CreateToken(decorator, result, CurlyTokenType.NameSeparator);

                decorator.ReadWhile(Char.IsLetterOrDigit);
            } 
            
            if(decorator.Current == ' ')
            {
                // Use as name.
                CreateToken(decorator, result, CurlyTokenType.Name, 1);

                decorator.ReadWhile(Char.IsWhiteSpace);
                CreateToken(decorator, result, CurlyTokenType.Whitespace, 1);
            }

            if (Char.IsLetter(decorator.Current))
            {
                // Attribute or default attribute.
                ReadTokenAttribute(decorator, context, result);
            }

            if (decorator.Current == '}')
            {
                // Use as name and close token.
                CreateToken(decorator, result, CurlyTokenType.Name, 1);
                CreateToken(decorator, result, CurlyTokenType.CloseBrace);
                
                // Read tokens and accept last characters as text.
                ReadTokenStart(decorator, context, result);
                CreateToken(decorator, result, CurlyTokenType.Text);
            }
        }

        private void ReadTokenAttribute(ContentDecorator decorator, IComposableTokenizerContext context, List<ComposableToken> result)
        {
            decorator.ReadWhile(Char.IsLetterOrDigit);

            if (decorator.Current == '=')
            {
                // Use as attribute name.
                CreateToken(decorator, result, CurlyTokenType.AttributeName, 1);
                CreateToken(decorator, result, CurlyTokenType.AttributeValueSeparator);

                // Use as attribute value.
                decorator.ReadUntil(c => c == ',' || c == '}');
                CreateToken(decorator, result, CurlyTokenType.AttributeValue, 1);

                if (decorator.Current == ',')
                    ReadTokenAttribute(decorator, context, result);

                if (decorator.Current == '}')
                    return;
            }
            else if (decorator.Current == ',')
            {
                // Use as default.
                CreateToken(decorator, result, CurlyTokenType.DefaultAttributeValue, 1);
                CreateToken(decorator, result, CurlyTokenType.AttributeSeparator);

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
                    LineInfo = decorator.CurrentLineInfo()
                });

                decorator.ResetCurrentInfo();
            }

            if (stepsToGoBack > 0)
                decorator.Read(stepsToGoBack);
        }
    }
}
