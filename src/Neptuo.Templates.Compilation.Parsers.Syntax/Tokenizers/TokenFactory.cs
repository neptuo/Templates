﻿using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers.ComponentModel;
using Neptuo.Text.Positions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers
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
    }
}
