using Neptuo.Text.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Text
{
    /// <summary>
    /// Util methods the every token builder needs.
    /// </summary>
    public abstract class TokenBuilderBase : ITokenBuilder
    {
        /// <summary>
        /// Processes <paramref name="decorator"/> and creates list of syntactic tokens.
        /// </summary>
        /// <param name="decorator">Input content decorator.</param>
        /// <param name="context">Context for input processing.</param>
        /// <returns>List of syntactic tokens from <paramref name="decorator"/>.</returns>
        public IList<Token> Tokenize(ContentDecorator decorator, ITokenBuilderContext context)
        {
            Ensure.NotNull(decorator, "decorator");
            Ensure.NotNull(context, "context");

            List<Token> result = new List<Token>();
            Tokenize(decorator, context, result);
            return result;
        }

        protected abstract void Tokenize(ContentDecorator decorator, ITokenBuilderContext context, List<Token> result);
    }
}
