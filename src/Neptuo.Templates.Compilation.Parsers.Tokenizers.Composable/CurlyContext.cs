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
        public List<ComposableToken> Result { get; private set; }

        public CurlyContext(ContentDecorator decorator, IComposableTokenizerContext context)
        {
            Result = new List<ComposableToken>();
            Decorator = decorator;
            TokenizerContext = context;
        }

        public ComposableTokenBuilder CreateToken(ComposableTokenType tokenType, int stepsToGoBack = -1, bool isSkipped = false)
        {
            if (stepsToGoBack > 0)
            {
                if (!Decorator.ResetCurrentPosition(stepsToGoBack))
                    throw Ensure.Exception.NotSupported("Unnable to process back steps.");
            }

            string text = Decorator.CurrentContent();
            if (!String.IsNullOrEmpty(text))
            {
                ComposableToken token = new ComposableToken(tokenType, text)
                {
                    ContentInfo = Decorator.CurrentContentInfo(),
                    LineInfo = Decorator.CurrentLineInfo(),
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

        public void CreateVirtualToken(ComposableTokenType tokenType, string text)
        {
            Result.Add(new ComposableToken(tokenType, text)
            {
                IsVirtual = true
            });
        }
    }
}
