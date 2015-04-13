using Neptuo.Collections.Specialized;
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

            /// <summary>
            /// ' '
            /// </summary>
            NameSeparator,
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

        public static class TokenType
        {
            public static readonly ComposableTokenType OpenBrace = new ComposableTokenType("Curly.OpenBrace");
            public static readonly ComposableTokenType Name = new ComposableTokenType("Curly.Name");

            public static readonly ComposableTokenType DefaultAttributeValue = new ComposableTokenType("Curly.DefaultAttributeName");
            public static readonly ComposableTokenType AttributeSeparator = new ComposableTokenType("Curly.AttributeSeparator");

            public static readonly ComposableTokenType CloseBrace = new ComposableTokenType("Curly.CloseBrace");
        }

        private State CurrentState(ComposableTokenizerContext context)
        {
            return context.CustomValues.Get<State>("Curly.State", State.Initial);
        }

        private void CurrentState(ComposableTokenizerContext context, State state)
        {
            context.CustomValues.Set("Curly.State", state);
        }

        public bool Accept(char input, ComposableTokenizerContext context)
        {
            State state = CurrentState(context);
            if (state == State.Initial)
            {
                if (input == '{')
                {
                    context.TryCreateToken(this, ComposableTokenType.Text, true);
                    context.TryCreateToken(this, TokenType.OpenBrace);
                    CurrentState(context, State.OpenBrace);
                    return true;
                }

                return false;
            }
            else if (state == State.OpenBrace)
            {
                if (input == '{')
                {
                    context.TryCreateToken(this, ComposableTokenType.Error);
                    return true;
                }

                if(input == '}')
                {
                    context.TryCreateToken(this, TokenType.Name, true);
                    context.TryCreateToken(this, TokenType.CloseBrace);
                    CurrentState(context, State.Initial);
                    return true;
                }

                if (input == ' ')
                {
                    context.TryCreateToken(this, TokenType.Name, true);
                    context.TryCreateToken(this, ComposableTokenType.Text);
                    CurrentState(context, State.NameSeparator);
                    return true;
                }

                if (Char.IsLetterOrDigit(input))
                    return true;
            }
            else if (state == State.NameSeparator)
            {
                if(input == '}')
                {
                    context.TryCreateToken(this, C, true);
                    context.TryCreateToken(this, TokenType.CloseBrace);
                    CurrentState(context, State.Initial);
                    return true;
                }

                if (Char.IsLetterOrDigit(input))
                    return true;
            }

            return false;
        }

        public void Finalize(ComposableTokenizerContext context)
        {
            context.TryCreateToken(this, ComposableTokenType.Error);
        }

        public IEnumerable<ComposableTokenType> GetSupportedTokenTypes()
        {
            return new List<ComposableTokenType>()
            {
                TokenType.OpenBrace,
                TokenType.Name,
                TokenType.CloseBrace
            };
        }
    }
}
