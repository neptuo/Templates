using Neptuo.Text.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Text
{
    /// <summary>
    /// Sub-token list creator/builder.
    /// </summary>
    public interface ITokenBuilder
    {
        /// <summary>
        /// Processes <paramref name="decorator"/> and creates list of syntactic tokens.
        /// </summary>
        /// <param name="decorator">Input content decorator.</param>
        /// <param name="context">Context for input processing.</param>
        /// <returns>List of syntactic tokens from <paramref name="decorator"/>.</returns>
        IList<Token> Tokenize(ContentDecorator decorator, ITokenBuilderContext context);
    }
}
