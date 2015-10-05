using Neptuo.Templates.Compilation.Parsers.Tokenizers.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Tokenizers
{
    internal class CurlyContext
    {
        public ContentDecorator Decorator { get; private set; }
        public IComposableTokenizerContext TokenizerContext { get; private set; }
        public List<Token> Result { get; private set; }

        public CurlyContext(List<Token> result, ContentDecorator decorator, IComposableTokenizerContext context)
        {
            Result = result;
            Decorator = decorator;
            TokenizerContext = context;
        }

        public ComposableTokenBuilder CreateToken(TokenType tokenType, int stepsToGoBack = -1, bool isSkipped = false)
        {
            if (stepsToGoBack > 0)
            {
                if (!Decorator.ResetCurrentPosition(stepsToGoBack))
                    throw Ensure.Exception.NotSupported("Unnable to process back steps.");
            }

            string text = Decorator.CurrentContent();
            if (!String.IsNullOrEmpty(text))
            {
                Token token = new Token(tokenType, text)
                {
                    TextSpan = Decorator.CurrentContentInfo(),
                    DocumentSpan = Decorator.CurrentLineInfo(),
                    IsSkipped = isSkipped
                };
                Result.Add(token);
                Decorator.ResetCurrentInfo();

                return new ComposableTokenBuilder(token);
            }

            if (stepsToGoBack > 0)
                Decorator.Read(stepsToGoBack);

            return new ComposableTokenBuilder();
        }

        public void CreateVirtualToken(TokenType tokenType, string text)
        {
            Result.Add(new Token(tokenType, text)
            {
                IsVirtual = true
            });
        }
    }
}
