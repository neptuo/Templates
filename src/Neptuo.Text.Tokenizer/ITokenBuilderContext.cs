using Neptuo.Text.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Text
{
    /// <summary>
    /// Context for <see cref="TokenBuilderBase"/>.
    /// </summary>
    public interface ITokenBuilderContext : ITokenizerContext
    {
        /// <summary>
        /// Processes <paramref name="decorator"/> and creates list of syntactic tokens.
        /// </summary>
        /// <param name="reader">Input content reader.</param>
        /// <param name="currentInfo">Offset info.</param>
        /// <returns>List of syntactic tokens from <paramref name="decorator"/>.</returns>
        IList<Token> Tokenize(IContentReader reader, ICurrentInfoAware currentInfo);
    }
}
