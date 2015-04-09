using Neptuo.Collections.Specialized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Tokenizers
{
    public class CurlyComposableTokenizer : IComposableTokenizer
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
            Whitespace,
            AttributeName,

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
                    context.TryCreateToken(this, TokenType.OpenBrace);
                    CurrentState(context, State.OpenBrace);
                    return true;
                }

                return false;
            }
            else if (state == State.OpenBrace)
            {
                if(input == '}')
                {
                    context.TryCreateToken(this, TokenType.Name, true);
                    context.TryCreateToken(this, TokenType.CloseBrace);
                    CurrentState(context, State.Initial);
                    //TODO: Include all? No!
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
    }
}
