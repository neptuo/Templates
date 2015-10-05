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
    public abstract class TokenizerBase
    {
        /// <summary>
        /// Processes <paramref name="decorator"/> and creates list of syntactic tokens.
        /// </summary>
        /// <param name="decorator">Input content decorator.</param>
        /// <param name="context">Context for input processing.</param>
        /// <returns>List of syntactic tokens from <paramref name="decorator"/>.</returns>
        public IList<Token> Tokenize(ContentDecorator decorator, IComposableTokenizerContext context)
        {
            Ensure.NotNull(decorator, "decorator");
            Ensure.NotNull(context, "context");

            List<Token> result = new List<Token>();
            Tokenize(decorator, context, result);
            return result;
        }

        protected abstract void Tokenize(ContentDecorator decorator, IComposableTokenizerContext context, List<Token> result);

        protected virtual void CreateToken(ContentDecorator decorator, List<Token> result, TokenType tokenType, int stepsToGoBack = -1)
        {
            if (stepsToGoBack > 0)
            {
                if (!decorator.ResetCurrentPosition(stepsToGoBack))
                    throw Ensure.Exception.NotSupported("Unnable to process back steps.");
            }

            string text = decorator.CurrentContent();
            if (!String.IsNullOrEmpty(text))
            {
                result.Add(new Token(tokenType, text)
                {
                    TextSpan = decorator.CurrentContentInfo(),
                    DocumentSpan = decorator.CurrentLineInfo(),
                });

                decorator.ResetCurrentInfo();
            }

            if (stepsToGoBack > 0)
                decorator.Read(stepsToGoBack);
        }

        protected virtual void CreateVirtualToken(List<Token> result, TokenType tokenType, string text)
        {
            result.Add(new Token(tokenType, text)
            {
                IsVirtual = true
            });
        }
    }
}
