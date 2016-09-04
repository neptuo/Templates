using Neptuo.Text.ComponentModel;
using Neptuo.Text.Positions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Text
{
    public class TokenFactory
    {
        private readonly Token token;

        public TokenFactory()
        {
            this.token = new Token();
        }

        public TokenFactory(Token token)
        {
            Ensure.NotNull(token, "token");
            this.token = token;
        }

        public TokenFactory WithText(string text)
        {
            token.Text = text;
            return this;
        }

        public TokenFactory WithType(TokenType type)
        {
            Ensure.NotNull(type, "type");
            token.Type = type;
            return this;
        }

        public TokenFactory WithError(string errorMessage)
        {
            token.Errors.Add(new DefaultErrorMessage(errorMessage));
            token.IsSkipped = !token.IsVirtual;
            return this;
        }

        public TokenFactory WithSkippedError(string errorMessage, bool isSkipped)
        {
            token.Errors.Add(new DefaultErrorMessage(errorMessage));
            token.IsSkipped = isSkipped;
            return this;
        }

        public TokenFactory WithTextSpan(int startIndex, int length)
        {
            token.TextSpan = new DefaultTextSpan(startIndex, length);
            return this;
        }

        public TokenFactory WithDocumentSpan(int lineIndex, int columnIndex, int endLineIndex, int endColumnIndex)
        {
            token.DocumentSpan = new DefaultDocumentSpan(lineIndex, columnIndex, endLineIndex, endColumnIndex);
            return this;
        }

        public Token ToToken()
        {
            return token;
        }

        public static implicit operator Token(TokenFactory factory)
        {
            Ensure.NotNull(factory, "factory");
            return factory.ToToken();
        }
    }
}
