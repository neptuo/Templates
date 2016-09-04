using Neptuo.Text.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Text
{
    internal class InternalContext
    {
        public ContentDecorator Decorator { get; private set; }
        public ITokenizerContext TokenizerContext { get; private set; }
        public List<Token> Result { get; private set; }

        public InternalContext(List<Token> result, ContentDecorator decorator, ITokenizerContext context)
        {
            Result = result;
            Decorator = decorator;
            TokenizerContext = context;
        }

        public TokenFactory CreateToken(TokenType tokenType, int stepsToGoBack = -1, bool isSkipped = false)
        {
            if (stepsToGoBack > 0)
            {
                if (!Decorator.ResetCurrentPosition(stepsToGoBack))
                    throw Ensure.Exception.NotSupported("Unnable to process back steps.");
            }

            Token token = null;
            string text = Decorator.CurrentContent();
            if (!String.IsNullOrEmpty(text))
            {
                token = new Token(tokenType, text)
                {
                    TextSpan = Decorator.CurrentContentInfo(),
                    DocumentSpan = Decorator.CurrentLineInfo(),
                    IsSkipped = isSkipped
                };
                Result.Add(token);
                Decorator.ResetCurrentInfo();
            }

            if (stepsToGoBack > 0)
                Decorator.Read(stepsToGoBack);

            if (token == null)
                return new TokenFactory();

            return new TokenFactory(token);
        }

        public TokenFactory CreateVirtualToken(TokenType tokenType, string text)
        {
            Token token = new Token(tokenType, text)
            {
                IsVirtual = true,
                TextSpan = Decorator.CurrentContentInfo(0)
            };
            Result.Add(token);

            return new TokenFactory(token);
        }
    }
}
