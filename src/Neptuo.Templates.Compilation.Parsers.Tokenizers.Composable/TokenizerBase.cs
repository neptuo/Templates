using Neptuo.Templates.Compilation.Parsers.Tokenizers.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Tokenizers
{
    /// <summary>
    /// Util methods the every tokenizer needs.
    /// </summary>
    public abstract class TokenizerBase : IComposableTokenizer
    {
        public IList<ComposableToken> Tokenize(ContentDecorator decorator, IComposableTokenizerContext context)
        {
            Ensure.NotNull(decorator, "decorator");
            Ensure.NotNull(context, "context");

            List<ComposableToken> result = new List<ComposableToken>();
            Tokenize(decorator, context, result);
            return result;
        }

        protected abstract void Tokenize(ContentDecorator decorator, IComposableTokenizerContext context, List<ComposableToken> result);

        protected virtual void CreateToken(ContentDecorator decorator, List<ComposableToken> result, ComposableTokenType tokenType, int stepsToGoBack = -1)
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
                    TextSpan = decorator.CurrentContentInfo(),
                    DocumentSpan = decorator.CurrentLineInfo(),
                });

                decorator.ResetCurrentInfo();
            }

            if (stepsToGoBack > 0)
                decorator.Read(stepsToGoBack);
        }

        protected virtual void CreateVirtualToken(List<ComposableToken> result, ComposableTokenType tokenType, string text)
        {
            result.Add(new ComposableToken(tokenType, text)
            {
                IsVirtual = true
            });
        }
    }
}
